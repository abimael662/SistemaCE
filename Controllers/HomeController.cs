using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using SistemaCE.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;

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

        public IActionResult Index()
        {
            try
            {
                int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                var datos = _context.Personas
                    .Include(p => p.Estudiante)
                        .ThenInclude(e => e.IdGrupoNavigation)
                    .Include(p => p.Docente)
                    .Include(p => p.Administrativo)
                    .FirstOrDefault(p => p.IdPersona == idUsuario);

                return View(datos);
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

