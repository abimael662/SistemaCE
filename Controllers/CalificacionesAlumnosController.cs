using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
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

        [Authorize(Roles = "estudiante,docente")]
        public async Task<IActionResult> Index()
        {

            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var docentematerias = await _context.DocenteMateria
                .Where(m => m.IdDocente == idUsuario)
                .Select(m => m.IdMateria)
                .ToListAsync();

            var calificaciones = await _context.CalificacionAlumnos
    .Include(c => c.IdDocenteNavigation)
        .ThenInclude(d => d.IdDocenteNavigation)
    .Include(c => c.IdEstudianteNavigation)
        .ThenInclude(e => e.IdEstudianteNavigation)
    .Include(c => c.IdMateriaNavigation)
    .Include(c => c.IdGrupoNavigation)
    .Where(c => docentematerias.Contains(c.IdMateriaNavigation.IdMateria) && c.IdDocente == idUsuario)
    .ToListAsync();


            return View(calificaciones);
        }

        [Authorize(Roles = "estudiante")]
        public async Task<IActionResult> General()
        {
            // Obtener ID del docente logueado
            //Convertimos a entero el idUsuario
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            return View(await _context.CalificacionAlumnos
                .Where(c => c.IdEstudiante == idUsuario)
                .Include(c => c.IdDocenteNavigation)
        .Include(c => c.IdEstudianteNavigation)
        .Include(c => c.IdMateriaNavigation)
                .ToListAsync());
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
            var docentematerias = _context.DocenteMateria
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
    }

}