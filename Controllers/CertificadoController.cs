using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaCE.Controllers
{
    public class CertificadoController : Controller
    {
        // GET: CertificadoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CertificadoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CertificadoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CertificadoController/Create
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

        // GET: CertificadoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CertificadoController/Edit/5
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

        // GET: CertificadoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CertificadoController/Delete/5
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
