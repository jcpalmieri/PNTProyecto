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

        // GET: Publicaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Publicaciones.ToListAsync());
        }

        // GET: Publicaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.nroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // GET: Publicaciones/Create
        public IActionResult Create()
        {
            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View();
        }

        // POST: Publicaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publicacion publicacion, IFormFile ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    // Guardar la imagen en el servidor (por ejemplo, en la carpeta wwwroot/images)
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagenFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenFile.CopyToAsync(stream);
                    }

                    // Almacenar la ruta de la imagen en el modelo
                    publicacion.Imagen = "/images/" + uniqueFileName; // Ajusta la ruta según tu estructura de carpeta

                    // Guardar el modelo en la base de datos
                    _context.Add(publicacion);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            // Si llegamos aquí, significa que hubo un error en el ModelState, volvemos a cargar la vista con el modelo y los ViewBag necesarios
            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View(publicacion);
        }



        // GET: Publicaciones/Edit/5
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

        // POST: Publicaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("nroPublicacion,NombreMascota,Descripcion,TipoMascota,Contacto,Imagen")] Publicacion publicacion, IFormFile imagenFile)
        {
            if (id != publicacion.nroPublicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Procesar la imagen si se ha subido
                    if (imagenFile != null && imagenFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imagenFile.CopyToAsync(memoryStream);
                            publicacion.Imagen = Convert.ToBase64String(memoryStream.ToArray()); // Convertir la imagen a base64
                        }
                    }

                    _context.Update(publicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicacionExists(publicacion.nroPublicacion))
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

        // GET: Publicaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones
                .FirstOrDefaultAsync(m => m.nroPublicacion == id);
            if (publicacion == null)
            {
                return NotFound();
            }

            return View(publicacion);
        }

        // POST: Publicaciones/Delete/5
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
            return _context.Publicaciones.Any(e => e.nroPublicacion == id);
        }
    }
}
