using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;

namespace SistemaCE.Controllers
{
    [Authorize]
    public class DocentesController : Controller
    {
        private readonly SceContext _context;

        public DocentesController(SceContext context)
        {
            _context = context;
        }

        // GET: Docentes
        public async Task<IActionResult> Index()
        {
            var sceContext = _context.Docentes.Include(d => d.IdDocenteNavigation).Include(d => d.NumeroEmpleadoNavigation);
            return View(await sceContext.ToListAsync());
        }

        // GET: Docentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .Include(d => d.IdDocenteNavigation)
                .Include(d => d.NumeroEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdDocente == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // GET: Docentes/Create
        public IActionResult Create()
        {
            // 🔥 DEBUG AQUÍ
            var personas = _context.Personas.ToList();
            var empleados = _context.Empleados.ToList();

            if (personas == null || !personas.Any())
            {
                throw new Exception("NO HAY PERSONAS");
            }

            if (empleados == null || !empleados.Any())
            {
                throw new Exception("NO HAY EMPLEADOS");
            }

            // 👇 YA TU CÓDIGO NORMAL
            var personasSelect = personas.Select(p => new
            {
                p.IdPersona,
                NombreCompleto = (p.Nombre ?? "") + " " + (p.ApellidoPaterno ?? "") + " " + (p.ApellidoMaterno ?? "")
            }).ToList();

            ViewBag.IdDocente = new SelectList(personasSelect, "IdPersona", "NombreCompleto", null);
            ViewBag.NumeroEmpleado = new SelectList(empleados, "IdEmpleado", "TipoEmpleado", null);

            return View();
        }
        // POST: Docentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Docentes/Create
        // POST: Docentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDocente,NumeroEmpleado,CedulaProfesional,Rfc,Sueldo")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                var persona = await _context.Personas.FindAsync(docente.IdDocente);

                if (persona == null)
                {
                    ModelState.AddModelError("", "La persona no existe");
                }
                else
                {
                    docente.IdDocenteNavigation = persona;

                    _context.Docentes.Add(docente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (!ModelState.IsValid)
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var e in errores)
                {
                    Console.WriteLine(e.ErrorMessage);
                }
            }

            // 🔁 RECARGAR SELECTS SI FALLA
            var personas = _context.Personas
                .Select(p => new
                {
                    p.IdPersona,
                    NombreCompleto = (p.Nombre ?? "") + " " + (p.ApellidoPaterno ?? "") + " " + (p.ApellidoMaterno ?? "")
                })
                .ToList();

            ViewBag.IdDocente = new SelectList(personas, "IdPersona", "NombreCompleto", docente.IdDocente);

            var empleados = _context.Empleados.ToList();
            ViewBag.NumeroEmpleado = new SelectList(empleados, "IdEmpleado", "TipoEmpleado", docente.NumeroEmpleado);
            //ViewBag.NumeroEmpleado = new SelectList(empleados, "IdEmpleado", "NumeroEmpleado", docente.NumeroEmpleado);

            return View(docente);
        }

        // GET: Docentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }
            ViewData["IdDocente"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", docente.IdDocente);
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", docente.NumeroEmpleado);
            return View(docente);
        }

        // POST: Docentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDocente,NumeroEmpleado,CedulaProfesional,Rfc,Sueldo")] Docente docente)
        {
            if (id != docente.IdDocente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocenteExists(docente.IdDocente))
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
            ViewData["IdDocente"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", docente.IdDocente);
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", docente.NumeroEmpleado);
            return View(docente);
        }

        // GET: Docentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes
                .Include(d => d.IdDocenteNavigation)
                .Include(d => d.NumeroEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdDocente == id);
            if (docente == null)
            {
                return NotFound();
            }

            return View(docente);
        }

        // POST: Docentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.IdDocente == id);
        }
    }
}
