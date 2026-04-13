using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Security.Claims;

namespace SistemaCE.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SceContext _context;

        public HomeController(SceContext context)
        {
            _context = context;
        }

        private async Task GuardarSesionUsuario(Persona persona)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role );

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
                return RedirectToAction("Main", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
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

        public IActionResult Main()
        {
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol == "docente")
            {
                return RedirectToAction("MainDocente" , "Home");
            }
            else if (rol == "estudiante")
            {
                return RedirectToAction("MainAlumno", "Home");
            }
            else
            {
                return RedirectToAction("MainAdmin", "Home");
            }
        }

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

        public IActionResult MainAdmin()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var datos = _context.Administrativos
                .Include(d => d.IdAdministrativoNavigation)
                .FirstOrDefault(d => d.IdAdministrativoNavigation.IdPersona == idUsuario);
            //System.InvalidOperationException: 'The expression 'd.IdAdministrativo' is invalid inside an 'Include' operation, since it does not represent a property access: 't => t.MyProperty'. To target navigations declared on derived types, use casting ('t => ((Derived)t).MyProperty') or the ' as' operator ('t => (t as Derived).MyProperty'). Collection navigation access can be filtered by composing Where, OrderBy(Descending), ThenBy(Descending), Skip or Take operations. For more information on including related data, see https://go.microsoft.com/fwlink/?LinkID=746393.'

            return View(datos);
        }
    }
}