using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace SistemaCE.Controllers
{
    public class CalificacionesAlumnosController : Controller
    {
        private readonly SceContext _context;

        public CalificacionesAlumnosController(SceContext context)
        {
            _context = context;
        }

        /// Prueba por si no sirve
        [HttpGet]
        [Authorize(Roles = "docente")]
        public async Task<IActionResult> Index(int? grupoId, int? materiaId)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var docente = await _context.Docentes
                .Where(d => d.IdDocente == idUsuario)
                .Select(d => d.IdDocenteNavigation!.Nombre + " " + d.IdDocenteNavigation.ApellidoPaterno + " " + d.IdDocenteNavigation.ApellidoMaterno)
                .FirstOrDefaultAsync();

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

            ViewBag.Docente = docente;
            ViewBag.Grupos = gruposDocente;
            ViewBag.Division = division;
            ViewBag.GrupoSeleccionado = grupoId;
            ViewBag.MateriaSeleccionada = materiaId;
            ViewBag.Materias = materias;

            /// Consulta para obtener estudiantes con sus calificaciones, incluyendo estudiantes sin calificaciones
            var estudiantesConCalificacionesQuery = from e in _context.Estudiantes
                                                    join p in _context.Personas on e.IdEstudiante equals p.IdPersona
                                                    join g in _context.Grupos on e.IdGrupo equals g.IdGrupo
                                                    join dm in _context.DocenteMateriaGrupos on g.IdGrupo equals dm.IdGrupo into dmJoin
                                                    from dm in dmJoin.DefaultIfEmpty()
                                                    join c in _context.CalificacionAlumnos
                                                    .Where(ca => ca.IdDocente == idUsuario)
                                                        on new { IdEstudiante = (int?)e.IdEstudiante, IdMateria = (int?)dm.IdMateria }
                                                        equals new { IdEstudiante = c.IdEstudiante, IdMateria = c.IdMateria } into cJoin
                                                    from cal in cJoin.DefaultIfEmpty()
                                                    join d in _context.Docentes on dm.IdDocente equals d.IdDocente
                                                    join dp in _context.Personas on d.IdDocente equals dp.IdPersona
                                                    where dm.IdDocente == idUsuario
                                                    select new CalificacionAlumno
                                                    {
                                                        IdCalificacion = cal != null ? cal.IdCalificacion : 0,
                                                        IdEstudiante = e.IdEstudiante,
                                                        IdEstudianteNavigation = new Estudiante { IdEstudianteNavigation = p },
                                                        IdMateria = dm.IdMateria,
                                                        IdMateriaNavigation = dm.IdMateriaNavigation,
                                                        IdGrupo = g.IdGrupo,
                                                        IdGrupoNavigation = g,
                                                        Parcial1 = cal != null ? cal.Parcial1 : null,
                                                        Parcial2 = cal != null ? cal.Parcial2 : null,
                                                        Parcial3 = cal != null ? cal.Parcial3 : null,
                                                        PromedioFinal = cal != null ? cal.PromedioFinal : null,
                                                        IdDocente = idUsuario,
                                                        IdDocenteNavigation = new Docente { IdDocenteNavigation = dp },
                                                    };

            if (grupoId.HasValue)
            {
                estudiantesConCalificacionesQuery = estudiantesConCalificacionesQuery
                                                    .Where(c => c.IdGrupo == grupoId.Value);
            }
            
            if (materiaId.HasValue)
            {
                estudiantesConCalificacionesQuery = estudiantesConCalificacionesQuery
                    .Where(c => c.IdMateria == materiaId.Value);
            }

            var estudiantesConCalificaciones = await estudiantesConCalificacionesQuery.ToListAsync();

            return View(estudiantesConCalificaciones);
        }

        [HttpPost]
        [Authorize(Roles = "docente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarCalificaciones(List<CalificacionAlumno> calificaciones)
        {
            int idDocente = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            foreach (var cal in calificaciones)
            {
                cal.Parcial1 ??= 0;
                cal.Parcial2 ??= 0;
                cal.Parcial3 ??= 0;

                decimal p1 = cal.Parcial1 ?? 0;
                decimal p2 = cal.Parcial2 ?? 0;
                decimal p3 = cal.Parcial3 ?? 0;

                cal.PromedioFinal = Math.Round((p1 + p2 + p3) / 3m, 2);

                var existente = await _context.CalificacionAlumnos
                    .FirstOrDefaultAsync(ca => ca.IdEstudiante == cal.IdEstudiante
                                            && ca.IdMateria == cal.IdMateria
                                            && ca.IdGrupo == cal.IdGrupo);

                if (existente != null)
                {
                    existente.Parcial1 = cal.Parcial1;
                    existente.Parcial2 = cal.Parcial2;
                    existente.Parcial3 = cal.Parcial3;
                    existente.PromedioFinal = cal.PromedioFinal;
                    existente.IdDocente = idDocente;
                    _context.Update(existente);
                }
                else
                {
                    cal.IdDocente = idDocente;
                    _context.Add(cal);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var mensajeReal = ex.InnerException?.Message;
                throw new Exception("Error real al guardar: " + mensajeReal);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "estudiante")]
        public async Task<IActionResult> HistorialCalificaciones(int? grupoId)
        {
            // Obtener ID del docente logueado
            //Convertimos a entero el idUsuario
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var alumno = await _context.Estudiantes
                .Where(e => e.IdEstudiante == idUsuario)
                .Include(e => e.IdEstudianteNavigation) // persona
                .Include(e => e.IdGrupoNavigation)
                    .ThenInclude(g => g.IdGrupoBaseNavigation)
                .Select(e => new
                {
                    NombreCompleto = e.IdEstudianteNavigation.Nombre + " " +
                                     e.IdEstudianteNavigation.ApellidoPaterno + " " +
                                     e.IdEstudianteNavigation.ApellidoMaterno,

                    Grupo = e.IdGrupoNavigation.IdGrupoBaseNavigation.Nombre,
                    Matricula = e.Matricula,
                    Estatus = e.Estatus
                })
                .FirstOrDefaultAsync();


            var grupos = await _context.CalificacionAlumnos
                .Where(c => c.IdEstudiante == idUsuario)
                .Select(c => c.IdGrupoNavigation)
                .Select(g => new {
                    IdGrupo = g.IdGrupo,
                    Nombre = g.IdGrupoBaseNavigation.Nombre
                })
                .Distinct()
                .ToListAsync();

            if (!grupoId.HasValue && grupos.Any())
            {
                grupoId = grupos.First().IdGrupo;
            }

            ViewBag.Grupos = grupos;
            ViewBag.GrupoSeleccionado = grupoId;
            ViewBag.Alumno = alumno;


            var query = _context.CalificacionAlumnos
                .Where(c => c.IdEstudiante == idUsuario);

            if (grupoId.HasValue)
                query = query.Where(c => c.IdGrupo == grupoId.Value);

            var resultado = await query
                .Include(c => c.IdDocenteNavigation)
                    .ThenInclude(d => d.IdDocenteNavigation)
                .Include(c => c.IdEstudianteNavigation)
                .Include(c => c.IdMateriaNavigation)
                .ToListAsync();

            return View(resultado);
        }
        [Authorize(Roles = "estudiante")]
        public async Task<IActionResult> EvaluacionesActuales()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var alumno = await _context.Estudiantes
                .Where(e => e.IdEstudiante == idUsuario)
                .Include(e => e.IdEstudianteNavigation) // persona
                .Include(e => e.IdGrupoNavigation)
                    .ThenInclude(g => g.IdGrupoBaseNavigation)
                .Select(e => new
                {
                    NombreCompleto = e.IdEstudianteNavigation.Nombre + " " +
                                     e.IdEstudianteNavigation.ApellidoPaterno + " " +
                                     e.IdEstudianteNavigation.ApellidoMaterno,

                    Grupo = e.IdGrupoNavigation.IdGrupoBaseNavigation.Nombre,
                    Matricula = e.Matricula,
                    Estatus = e.Estatus
                })
                .FirstOrDefaultAsync();

            var grupoActual = await _context.Estudiantes
                .Where(e => e.IdEstudiante == idUsuario)
                .Select(e => e.IdGrupo)
                .FirstOrDefaultAsync();

            var calificaciones = await _context.CalificacionAlumnos
                .Where(c => c.IdEstudiante == idUsuario && c.IdGrupo == grupoActual)
                .Include(c => c.IdDocenteNavigation)
                    .ThenInclude(d => d.IdDocenteNavigation)
                .Include(c => c.IdMateriaNavigation)
                .ToListAsync();

            ViewBag.Alumno = alumno;

            return View(calificaciones);
        }


        [Authorize(Roles = "estudiante")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.CalificacionAlumnos
                .FirstOrDefaultAsync(m => m.IdCalificacion == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        [Authorize(Roles = "docente")]
        public IActionResult Create()
        {
            var personas = _context.Personas.ToList();
            var materias = _context.Materias.ToList();

            /// Cosas que agregue jajaja No lo hizo ChatGpt @GG
            var grupos = _context.Grupos.ToList();
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var docentematerias = _context.DocenteMateriaGrupos
                .Where(m => m.IdDocente == idUsuario)
                .Select(m => m.IdMateria)
                .ToList();

            if (!personas.Any())
                throw new Exception("NO HAY PERSONAS");

            var estudiantes = _context.Estudiantes
                .Where(e => personas.Select(p => p.IdPersona).Contains(e.IdEstudiante))
                .ToList();

            var estudiantesSelect = estudiantes.Join(personas, e => e.IdEstudiante, p => p.IdPersona, (e, p) => new
            {
              e.IdEstudiante,
              NombreCompleto = (p.Nombre ?? "") + " " + (p.ApellidoPaterno ?? "") + " " + (p.ApellidoMaterno ?? "")
            })
              .ToList();

            if (docentematerias == null)
            {
                return NotFound();
            }

            var materiasSelect = materias.Where(m => docentematerias.Contains(m.IdMateria)).ToList();

            ViewBag.IdGrupo = new SelectList(grupos, "IdGrupo", "Grupo1");
            ViewBag.IdMateria = new SelectList(materiasSelect, "IdMateria", "Nombre");
            ViewBag.IdEstudiante = new SelectList(estudiantesSelect, "IdEstudiante", "NombreCompleto");

            return View();
        }

        [Authorize(Roles = "docente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstudiante,IdMateria,IdGrupo,Parcial1")] CalificacionAlumno calificacionAlumno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    /// Validamos si el estudiante ya posee una calificacion
                    bool existe = await _context.CalificacionAlumnos
                        .AnyAsync(c => c.IdEstudiante == calificacionAlumno.IdEstudiante
                        && c.IdMateria == calificacionAlumno.IdMateria);

                    if (existe)
                    {
                        ModelState.AddModelError(string.Empty, "Este estudiante ya tiene calificación registrada para esta materia.");
                        var materias = _context.Materias.ToList();
                        var estudiantes = _context.Estudiantes.ToList();

                        /// Yo agregue esto
                        var grupos = _context.Grupos.ToList();

                        ViewBag.IdGrupo = new SelectList(grupos, "IdGrupo", "Grupo1", calificacionAlumno.IdGrupo);
                        ViewBag.IdMateria = new SelectList(materias, "IdMateria", "Nombre", calificacionAlumno.IdMateria);
                        ViewBag.IdEstudiante = new SelectList(estudiantes, "IdEstudiante", "IdEstudiante", calificacionAlumno.IdEstudiante);
                        return View(calificacionAlumno);
                    }

                    /// Obtener Id del docente logueado
                    int idDocente = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                    /// Asignar el ID del docente a la calificación
                    calificacionAlumno.IdDocente = idDocente;

                    /// Agregar la calificación al contexto y guardar cambios
                    _context.Add(calificacionAlumno);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(calificacionAlumno);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante la creación
                ModelState.AddModelError(string.Empty, $"Ocurrió un error al crear la calificación: {ex.Message}");
                return View(calificacionAlumno);
            }
        }

        [Authorize(Roles = "docente")]
        // GET: Divisions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.CalificacionAlumnos.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }
            return View(division);
        }

        [Authorize(Roles = "docente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDivision,Nombre,Nomenclatura")] Division division)
        {
            if (id != division.IdDivision)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(division);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionExists(division.IdDivision))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(division);
        }

        // GET: Divisions/Delete/5
        [Authorize(Roles = "docente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .FirstOrDefaultAsync(m => m.IdDivision == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Divisions/Delete/5
        [Authorize(Roles = "docente")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division != null)
            {
                _context.Divisions.Remove(division);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return _context.Divisions.Any(e => e.IdDivision == id);
        }

        //[Authorize(Roles = "estudiante")]
        //public IActionResult EvaluacionesActuales()
        //{
        //    int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        //    var data = _context.CalificacionAlumnos
        //        .Include(c => c.IdMateriaNavigation)
        //        .Include(c => c.IdDocenteNavigation)
        //        .Include(c=> c.IdGrupoNavigation)
        //        .Where(c => c.IdEstudiante == idUsuario)
        //        .ToList();

        //    return View(data);
        //}
    }

}