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
    [Authorize(Roles = "administrativo")]
    public class AdministrativoesController : Controller
    {
        private readonly SceContext _context;

        public AdministrativoesController(SceContext context)
        {
            _context = context;
        }

        // GET: Administrativoes
        public async Task<IActionResult> Index()
        {
            var sceContext = _context.Administrativos.Include(a => a.IdAdministrativoNavigation).Include(a => a.NumeroEmpleadoNavigation);
            return View(await sceContext.ToListAsync());
        }

        // GET: Administrativoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrativo = await _context.Administrativos
                .Include(a => a.IdAdministrativoNavigation)
                .Include(a => a.NumeroEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdAdministrativo == id);
            if (administrativo == null)
            {
                return NotFound();
            }

            return View(administrativo);
        }

        // GET: Administrativoes/Create
        public IActionResult Create()
        {
            var personas = _context.Personas.ToList();
            var empleados = _context.Empleados.ToList();

            var personasOcupadas = _context.Docentes.Select(d => d.IdDocente)
                .Union(_context.Administrativos.Select(a => a.IdAdministrativo))
                .Union(_context.Estudiantes.Select(e => e.IdEstudiante))
                .ToList();

            var personasDisponibles = personas
                .Where(p => !personasOcupadas.Contains(p.IdPersona))
                .ToList();

            if (!personasDisponibles.Any())
            {
                ViewData["IdAdministrativo"] = new SelectList(Enumerable.Empty<object>());
            }
            else
            {
                var personasSelect = personasDisponibles.Select(p => new
                {
                    p.IdPersona,
                    NombreCompleto = (p.Nombre ?? "") + " " +
                                     (p.ApellidoPaterno ?? "") + " " +
                                     (p.ApellidoMaterno ?? "")
                }).ToList();

                ViewData["IdAdministrativo"] = new SelectList(personasSelect, "IdPersona", "NombreCompleto", null);
            }

            ViewData["NumeroEmpleado"] = new SelectList(empleados, "IdEmpleado", "NumeroEmpleado", null);

            return View();
        }
        // POST: Administrativoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAdministrativo,Departamento,Puesto,Rfc,Sueldo")] Administrativo administrativo)
        {
            if (ModelState.IsValid)
            {
                var persona = await _context.Personas
                    .FirstOrDefaultAsync(p => p.IdPersona == administrativo.IdAdministrativo);

                if (persona == null)
                {
                    ModelState.AddModelError("", "La persona no existe");
                    return View(administrativo);
                }

                string area = "SEC";
                var random = new Random();
                string numeroEmpleado;

                do
                {
                    int numero = random.Next(0, 10000);
                    numeroEmpleado = $"{area}{numero:D7}";
                }
                while (await _context.Empleados.AnyAsync(e => e.TipoEmpleado == numeroEmpleado));

                var empleado = new Empleado
                {
                    TipoEmpleado = numeroEmpleado
                };

                await _context.Empleados.AddAsync(empleado);
                await _context.SaveChangesAsync();

                administrativo.IdAdministrativo = persona.IdPersona;
                administrativo.NumeroEmpleado = empleado.IdEmpleado;

                await _context.Administrativos.AddAsync(administrativo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // RECARGAR DROPDOWNS
            var personas = _context.Personas
                .Select(p => new
                {
                    p.IdPersona,
                    NombreCompleto = (p.Nombre ?? "") + " " +
                                     (p.ApellidoPaterno ?? "") + " " +
                                     (p.ApellidoMaterno ?? "")
                })
                .ToList();

            ViewData["IdAdministrativo"] =
                new SelectList(personas, "IdPersona", "NombreCompleto", administrativo.IdAdministrativo);

            var empleados = _context.Empleados.ToList();

            ViewData["NumeroEmpleado"] =
                new SelectList(empleados, "IdEmpleado", "TipoEmpleado", administrativo.NumeroEmpleado);

            return View(administrativo);
        }

        // GET: Administrativoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrativo = await _context.Administrativos.FindAsync(id);
            if (administrativo == null)
            {
                return NotFound();
            }

            ViewData["IdAdministrativo"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", administrativo.IdAdministrativo);
            //ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "TipoEmpleado", administrativo.NumeroEmpleado);

            return View(administrativo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAdministrativo,Departamento,Puesto,Rfc,Sueldo")] Administrativo administrativo)
        {
            if (id != administrativo.IdAdministrativo)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(administrativo);
            }

            var existente = await _context.Administrativos
                .FirstOrDefaultAsync(a => a.IdAdministrativo == id);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Departamento = administrativo.Departamento;
            existente.Puesto = administrativo.Puesto;
            existente.Rfc = administrativo.Rfc;
            existente.Sueldo = administrativo.Sueldo;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Administrativoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrativo = await _context.Administrativos
                .Include(a => a.IdAdministrativoNavigation)
                .Include(a => a.NumeroEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdAdministrativo == id);
            if (administrativo == null)
            {
                return NotFound();
            }

            return View(administrativo);
        }

        // POST: Administrativoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrativo = await _context.Administrativos.FindAsync(id);
            if (administrativo != null)
            {
                _context.Administrativos.Remove(administrativo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministrativoExists(int id)
        {
            return _context.Administrativos.Any(e => e.IdAdministrativo == id);
        }
    }
}
