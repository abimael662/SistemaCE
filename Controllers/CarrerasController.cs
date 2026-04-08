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
    public class CarrerasController : Controller
    {
        private readonly SceContext _context;

        public CarrerasController(SceContext context)
        {
            _context = context;
        }

        // GET: Carreras
        public async Task<IActionResult> Index()
        {
            var sceContext = _context.Carreras.Include(c => c.IdDivisionNavigation).Include(c => c.IdNivelNavigation);
            return View(await sceContext.ToListAsync());
        }

        // GET: Carreras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras
                .Include(c => c.IdDivisionNavigation)
                .Include(c => c.IdNivelNavigation)
                .FirstOrDefaultAsync(m => m.IdCarrera == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // GET: Carreras/Create
        public IActionResult Create()
        {
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "IdDivision");
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "IdNivel");
            return View();
        }

        // POST: Carreras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarrera,Nombre,IdNivel,IdDivision")] Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "IdDivision", carrera.IdDivision);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "IdNivel", carrera.IdNivel);
            return View(carrera);
        }

        // GET: Carreras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "IdDivision", carrera.IdDivision);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "IdNivel", carrera.IdNivel);
            return View(carrera);
        }

        // POST: Carreras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarrera,Nombre,IdNivel,IdDivision")] Carrera carrera)
        {
            if (id != carrera.IdCarrera)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarreraExists(carrera.IdCarrera))
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
            ViewData["IdDivision"] = new SelectList(_context.Divisions, "IdDivision", "IdDivision", carrera.IdDivision);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "IdNivel", carrera.IdNivel);
            return View(carrera);
        }

        // GET: Carreras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras
                .Include(c => c.IdDivisionNavigation)
                .Include(c => c.IdNivelNavigation)
                .FirstOrDefaultAsync(m => m.IdCarrera == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // POST: Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                _context.Carreras.Remove(carrera);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarreraExists(int id)
        {
            return _context.Carreras.Any(e => e.IdCarrera == id);
        }
    }
}
