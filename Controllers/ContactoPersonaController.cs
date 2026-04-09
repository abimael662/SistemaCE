using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;

namespace SistemaCE.Controllers
{
    [Authorize]
    public class ContactoPersonaController : Controller
    {
        public readonly SceContext _context;

        public ContactoPersonaController(SceContext context) {
            _context = context;
        }

        // GET: ContactoPersonaController
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactoPersonas.ToListAsync());
        }

        // GET: ContactoPersonaController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.ContactoPersonas
                .FirstOrDefaultAsync(m => m.IdContacto == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }
        // GET: ContactoPersonaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactoPersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,TipoComunicacion,Dato")] ContactoPersona contactoPersona)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(contactoPersona);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(contactoPersona);
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactoPersonaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactoPersonaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ContactoPersonaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactoPersonaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
