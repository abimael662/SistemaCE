using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Security.Claims;

namespace SistemaCE.Controllers
{
    [Authorize]
    public class AsistenciaController : Controller
    {
        private readonly SceContext _context;
        public AsistenciaController(SceContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "docente")]
        // GET: AsistenciaController
        public async Task<IActionResult> Index(int? grupoId, int? materiaId)
        {

            /// Obtener el ID del usuario actual desde los claims @GG
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            /// Obtener el nombre completo del docente utilizando el ID del usuario @GG
            var docente = await _context.Docentes
                .Where(d => d.IdDocente == idUsuario)
                .Select(d => d.IdDocenteNavigation!.Nombre + " " + d.IdDocenteNavigation.ApellidoPaterno + " " + d.IdDocenteNavigation.ApellidoMaterno)
                .FirstOrDefaultAsync();

            /// Obtener los grupos asociados al docente utilizando el ID del usuario @GG
            var gruposDocente = await _context.DocenteMateriaGrupos
                .Where(d => d.IdDocente == idUsuario)
                .Select(d => d.IdGrupoNavigation)
                .Where(g => g != null)
                .Select(g => new {
                    IdGrupo = g.IdGrupo,
                    Nombre = g.IdGrupoBaseNavigation.Nombre
                })
                .Distinct()
                .ToListAsync();

            var materias = await (from d in _context.DocenteMateriaGrupos
                                  join m in _context.Materias
                                  on d.IdMateria equals m.IdMateria
                                  where d.IdDocente == idUsuario
                                  select m).Distinct().ToListAsync();

            /// Obtener la división a la que pertenece el docente utilizando el ID del usuario @GG
            var division = await (from c in _context.Docentes
                                  join d in _context.DocenteMateriaGrupos
                                      on c.IdDocente equals d.IdDocente
                                  join e in _context.Grupos
                                      on d.IdGrupo equals e.IdGrupo
                                  join f in _context.Carreras
                                      on e.IdCarrera equals f.IdCarrera
                                  join g in _context.Divisions
                                      on f.IdDivision equals g.IdDivision
                                  where c.IdDocente == idUsuario
                                  select g.Nombre)
                    .FirstOrDefaultAsync();

            if (!grupoId.HasValue && gruposDocente.Any())
            {
                grupoId = gruposDocente.First().IdGrupo;
            }

            if (!materiaId.HasValue && materias.Any())
            {
                materiaId = materias.First().IdMateria;
            }

            /// Pasar los datos obtenidos a la vista utilizando ViewBag @GG
            ViewBag.Docente = docente;
            ViewBag.Grupos = gruposDocente;
            ViewBag.Division = division;
            ViewBag.GrupoSeleccionado = grupoId;
            ViewBag.MateriaSeleccionada = materiaId;
            ViewBag.Materias = materias;

            var sesiones = await _context.SesionClases
                .Where(s => s.IdDocente == idUsuario)
                .ToListAsync();

            /// Obtener las asistencias relacionadas con las sesiones del docente utilizando el ID del usuario @GG
            var asistenciasQuery = from e in _context.Estudiantes
                                   join p in _context.Personas
                                       on e.IdEstudiante equals p.IdPersona
                                   join g in _context.Grupos
                                       on e.IdGrupo equals g.IdGrupo
                                   join dm in _context.DocenteMateriaGrupos
                                       on g.IdGrupo equals dm.IdGrupo
                                   join m in _context.Materias
                                       on dm.IdMateria equals m.IdMateria
                                   select new Asistencium
                                   {
                                       IdEstudiante = e.IdEstudiante,

                                       IdEstudianteNavigation = new Estudiante
                                       {
                                           IdEstudianteNavigation = p
                                       },

                                       IdSesion = 0,
                                       IdEstado = 1,
                                       Observacion = "",

                                       IdSesionNavigation = new SesionClase
                                       {
                                           IdGrupoNavigation = g,
                                           Fecha = DateOnly.FromDateTime(DateTime.Now),
                                           IdMateria = m.IdMateria

                                       }
                                   };

            if (grupoId.HasValue)
            {
                asistenciasQuery = asistenciasQuery
                    .Where(a => a.IdSesionNavigation.IdGrupoNavigation.IdGrupo == grupoId.Value);
            }
            if (materiaId.HasValue)
            {
                asistenciasQuery = asistenciasQuery
                    .Where(a => a.IdSesionNavigation.IdMateria == materiaId.Value);
            }
            var lista = await asistenciasQuery.ToListAsync();

            var estados = await _context.EstadoAsistencia.ToListAsync();
            ViewBag.Estados = new SelectList(estados, "IdEstado", "Nombre", null);

            return View(lista);
        }

        /// Opcion tentadora analizarla y checar @GG
        //public IActionResult Index()
        //{
        //    if (User.IsInRole("docente"))
        //        return View("IndexDocente");

        //    if (User.IsInRole("estudiante"))
        //        return View("IndexEstudiante");

        //    return Unauthorized();
        //}s

        [HttpPost]
        [Authorize(Roles = "docente")]
        public async Task<IActionResult> GuardarAsistencia(List<Asistencium> asistencias, DateOnly fecha, int idGrupo, int idMateria)
        { 
            int idDocente = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var fechaSeleccionada = fecha;

            // 🔍 Buscar sesión existente
            var sesion = await _context.SesionClases
                .FirstOrDefaultAsync(s =>
                    s.IdDocente == idDocente &&
                    s.IdGrupo == idGrupo &&
                    s.Fecha == fechaSeleccionada);

            // 🧠 Crear si no existe
            if (sesion == null)
            {
                sesion = new SesionClase
                {
                    IdDocente = idDocente,
                    IdGrupo = idGrupo,
                    IdMateria = idMateria,
                    Fecha = fechaSeleccionada,
                    HoraInicio = TimeOnly.FromDateTime(DateTime.Now),
                    HoraFin = TimeOnly.FromDateTime(DateTime.Now)
                };

                _context.SesionClases.Add(sesion);
                await _context.SaveChangesAsync();
            }

            // 🧠 Guardar o actualizar asistencias
            foreach (var a in asistencias)
            {
                var existente = await _context.Asistencia
                    .FirstOrDefaultAsync(x =>
                        x.IdEstudiante == a.IdEstudiante &&
                        x.IdSesion == sesion.IdSesion);

                if (existente != null)
                {
                    existente.IdEstado = a.IdEstado;
                    existente.Observacion = a.Observacion;
                    _context.Update(existente);
                }
                else
                {
                    _context.Asistencia.Add(new Asistencium
                    {
                        IdSesion = sesion.IdSesion,
                        IdEstudiante = a.IdEstudiante,
                        IdEstado = a.IdEstado,
                        Observacion = a.Observacion
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "estudiante")]
        public ActionResult IndexEstudiante()
        {
            return View();
        }

        // GET: AsistenciaController/Details/5
        [HttpGet]
        [Authorize(Roles = "estudiante")]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "docente")]
        // GET: AsistenciaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AsistenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "docente")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AsistenciaController/Edit/5
        [Authorize(Roles = "docente")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AsistenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "docente")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AsistenciaController/Delete/5
        [Authorize(Roles = "administrativo")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AsistenciaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrativo")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
