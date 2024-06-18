using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNTProyecto.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PNTProyecto.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacion
        public IActionResult Index()
        {
            return View(_context.Publicaciones.ToList());
        }

        // GET: Publicacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.NroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // GET: Publicacion/Create
        public IActionResult Create()
        {
            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View();
        }

        // POST: Publicacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreMascota,Descripcion,TipoMascota,Contacto")] Publicacion publicacion, IFormFile imagen)
        {
            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imagen.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }
                    publicacion.Imagen = "/images/" + imagen.FileName;
                }

                _context.Add(publicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View(publicacion);
        }

        // GET: Publicacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }
            return View(publicacion);
        }

        // POST: Publicacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroPublicacion,NombreMascota,Descripcion,TipoMascota,Contacto,Imagen")] Publicacion publicacion)
        {
            if (id != publicacion.NroPublicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicacionExists(publicacion.NroPublicacion))
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
            return View(publicacion);
        }

        // GET: Publicacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.NroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // POST: Publicacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return NotFound();
            }

            _context.Publicaciones.Remove(publicacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicaciones.Any(e => e.NroPublicacion == id);
        }
    }
}
