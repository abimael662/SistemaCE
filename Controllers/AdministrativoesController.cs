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
            ViewData["IdAdministrativo"] = new SelectList(_context.Personas, "IdPersona", "IdPersona");
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado");
            return View();
        }

        // POST: Administrativoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAdministrativo,NumeroEmpleado,Departamento,Puesto,Rfc,Sueldo")] Administrativo administrativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(administrativo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAdministrativo"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", administrativo.IdAdministrativo);
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", administrativo.NumeroEmpleado);
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
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", administrativo.NumeroEmpleado);
            return View(administrativo);
        }

        // POST: Administrativoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAdministrativo,NumeroEmpleado,Departamento,Puesto,Rfc,Sueldo")] Administrativo administrativo)
        {
            if (id != administrativo.IdAdministrativo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrativoExists(administrativo.IdAdministrativo))
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
            ViewData["IdAdministrativo"] = new SelectList(_context.Personas, "IdPersona", "IdPersona", administrativo.IdAdministrativo);
            ViewData["NumeroEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", administrativo.NumeroEmpleado);
            return View(administrativo);
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
