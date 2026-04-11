using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;

namespace SistemaCE.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly SceContext _context;

        public EstudiantesController(SceContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var estudiantes = await _context.Estudiantes
                .Include(e => e.IdEstudianteNavigation)
                .Include(e => e.IdGrupoNavigation)
                .ToListAsync();

            return View(estudiantes);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.IdEstudianteNavigation)
                .Include(e => e.IdGrupoNavigation)
                .FirstOrDefaultAsync(m => m.IdEstudiante == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            // 🔥 SOLO PERSONAS QUE NO SON ESTUDIANTES
            //var personasDisponibles = _context.Personas
            //    .Where(p => !_context.Estudiantes
            //        .Any(e => e.IdEstudiante == p.IdPersona))
            //    .ToList();

            /// Obtenemos todas las personas @GG
            var personas = _context.Personas.ToList();

            /// Verificamos que los usuarios que aparescan no esten registrados @GG
            var personasOcupadas = _context.Estudiantes.Select(d => d.IdEstudiante)
                .Union(_context.Administrativos.Select(a => a.IdAdministrativo))
                .Union(_context.Docentes.Select(e => e.IdDocente))
                .ToList();

            /// Seleccionamos a todas las personas que podemos registrar como estudiante @GG
            var personasDisponibles = personas
                .Where(p => !personasOcupadas.Contains(p.IdPersona))
                .ToList();

            ViewData["IdEstudiante"] = new SelectList(personasDisponibles, "IdPersona", "Nombre");
            ViewData["IdGrupo"] = new SelectList(_context.Grupos, "IdGrupo", "Grupo1");
            ViewData["Estatus"] = new SelectList(
                Enum.GetValues(typeof(EstatusEstudiante))
                .Cast<EstatusEstudiante>()
                .Select(e => new
                {Id = (int)e,Nombre = e.ToString()
                }),"Id","Nombre");

            return View();
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstudiante,IdGrupo,FechaIngreso,Estatus")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 🔥 BUSCAR la persona existente
                    var persona = await _context.Personas
                        .FirstOrDefaultAsync(p => p.IdPersona == estudiante.IdEstudiante);

                    if (persona == null)
                    {
                        ModelState.AddModelError("", "La persona no existe.");
                    }
                    else
                    {

                        /// Generar número único @GG
                        var random = new Random();
                        int matricula = random.Next(0, 1000000000);

                        estudiante.Matricula = matricula;
                        // 🔥 ASIGNAR la relación correctamente
                        estudiante.IdEstudianteNavigation = persona;

                        _context.Estudiantes.Add(estudiante);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            // recargar selects
            var personasDisponibles = _context.Personas
                .Where(p => !_context.Estudiantes
                    .Any(e => e.IdEstudiante == p.IdPersona))
                .ToList();

            ViewData["IdEstudiante"] = new SelectList(personasDisponibles, "IdPersona", "Nombre", estudiante.IdEstudiante);
            ViewData["IdGrupo"] = new SelectList(_context.Grupos, "IdGrupo", "Grupo1", estudiante.IdGrupo);

            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            ViewData["IdEstudiante"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", estudiante.IdEstudiante);
            ViewData["IdGrupo"] = new SelectList(_context.Grupos, "IdGrupo", "IdGrupo", estudiante.IdGrupo);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstudiante,IdGrupo,Matricula,FechaIngreso,Estatus")] Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.IdEstudiante))
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
            ViewData["IdEstudiante"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", estudiante.IdEstudiante);
            ViewData["IdGrupo"] = new SelectList(_context.Grupos, "IdGrupo", "IdGrupo", estudiante.IdGrupo);
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.IdEstudianteNavigation)
                .Include(e => e.IdGrupoNavigation)
                .FirstOrDefaultAsync(m => m.IdEstudiante == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.IdEstudiante == id);
        }
    }
}
