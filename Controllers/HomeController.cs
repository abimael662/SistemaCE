using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Security.Claims;

namespace SistemaCE.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly SceContext _context;

        public HomeController(SceContext context)
        {
            _context = context;
        }

        private async Task GuardarSesionUsuario(Persona persona)
        {

            var identity = new ClaimsIdentity(
           CookieAuthenticationDefaults.AuthenticationScheme,
           ClaimTypes.Name,
           ClaimTypes.Role
           );

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, persona.IdPersona.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, persona.Nombre!));
            identity.AddClaim(new Claim("ApellidoPaterno", persona.ApellidoPaterno!));
            identity.AddClaim(new Claim("ApellidoMaterno", persona.ApellidoMaterno!));

            if (_context.Docentes.Any(d => d.IdDocente == persona.IdPersona))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "docente"));
            }
            else if (_context.Estudiantes.Any(e => e.IdEstudiante == persona.IdPersona))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "estudiante"));
            }
            else
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "administrativo"));
            }

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddDays(1), IsPersistent = true });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Correo y contraseña son requeridos";
                    return View("Index");
                }

                var persona = _context.Personas
                    .Where(p => p.PersonaUsuario != null &&
                    p.PersonaUsuario.Usuario == usuario &&
                                p.PersonaUsuario.Password == password)
                    .FirstOrDefault();

                if (persona != null)
                {
                    await GuardarSesionUsuario(persona);
                    return RedirectToAction("Main", "Home");
                }

                ViewBag.Error = "Credenciales incorrectas";
                return View("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Main()
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol == "docente")
            {
                return RedirectToAction("MainDocente");
            }
            else if (rol == "estudiante")
            {
                return RedirectToAction("MainAlumno");
                //return View("MainAlumno");
            }
            else
            {
                return View("MainAdmin");
            }
        }

        [Authorize]
        public IActionResult MainAlumno()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var datos = _context.Personas
                .Include(p => p.Estudiante)
                    .ThenInclude(e => e.IdGrupoNavigation)
                    .ThenInclude(j => j.IdGrupoBaseNavigation)
                .FirstOrDefault(p => p.IdPersona == idUsuario);

            return View(datos);
        }
        public IActionResult MainDocente()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var datos = _context.Docentes
                .Include(d => d.IdDocenteNavigation)
                .FirstOrDefault(d => d.IdDocenteNavigation.IdPersona == idUsuario);

            return View(datos);
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
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

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
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
