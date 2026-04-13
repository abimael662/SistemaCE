using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Security.Claims;

namespace SistemaCE.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly SceContext _context;

        public LoginController(SceContext context)
        {
            _context = context;
        }

        private async Task GuardarSesionUsuario(Persona persona)
        {

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, persona.IdPersona.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, persona.Nombre!));

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
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Correo y contraseña son requeridos";
                return View();
            }

            var personaUsuario = await _context.PersonaUsuarios
    .Include(p => p.IdPersonaNavigation)
    .FirstOrDefaultAsync(p => p.Usuario == usuario);

            if (personaUsuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            var hasher = new PasswordHasher<PersonaUsuario>();

            var result = hasher.VerifyHashedPassword(
                personaUsuario,
                personaUsuario.Password,
                password
            );

            if (result == PasswordVerificationResult.Success)
            {
                var persona = personaUsuario.IdPersonaNavigation;

                if (persona == null)
                {
                    ViewBag.Error = "No se encontró la persona asociada";
                    return View();
                }

                await GuardarSesionUsuario(persona);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> Login(string usuario, string password)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        //        {
        //            ViewBag.Error = "Correo y contraseña son requeridos";
        //            return View();
        //        }

        //        var persona = _context.Personas
        //            .Where(p => p.PersonaUsuario != null &&
        //            p.PersonaUsuario.Usuario == usuario &&
        //                        p.PersonaUsuario.Password == password)
        //            .FirstOrDefault();

        //        if (persona != null)
        //        {
        //            await GuardarSesionUsuario(persona);
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.Error = "Credenciales incorrectas";
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
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
