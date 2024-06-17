using Microsoft.AspNetCore.Mvc;
using PNTProyecto.Models;
using System.Linq;

namespace PNTProyecto.Controllers
{
    public class PublicacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacion
        public IActionResult Index()
        {
            var publicaciones = _context.Publicaciones.ToList();
            return View(publicaciones);
        }

        // GET: Publicacion/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = _context.Publicaciones.FirstOrDefault(m => m.nroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // GET: Publicacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publicacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("nroPublicacion,nombreMascota,descripcion,imagen")] Publicacion publicacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publicacion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publicacion);
        }

        // GET: Publicacion/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = _context.Publicaciones.Find(id);
            if (publicacion == null)
            {
                return NotFound();
            }
            return View(publicacion);
        }

        // POST: Publicacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("nroPublicacion,nombreMascota,descripcion,imagen")] Publicacion publicacion)
        {
            if (id != publicacion.nroPublicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(publicacion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publicacion);
        }

        // GET: Publicacion/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = _context.Publicaciones.FirstOrDefault(m => m.nroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // POST: Publicacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var publicacion = _context.Publicaciones.Find(id);
            _context.Publicaciones.Remove(publicacion);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
