using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNTProyecto.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PNTProyecto.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PublicacionesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publicacion publicacion)
        {
            if (ModelState.IsValid)
            {
                // Obtener el nombre de usuario del usuario autenticado
                var userName = User.Identity.Name;

                // Buscar el UsuarioId en la base de datos
                var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

                if (usuario != null)
                {
                    // Asignar el UsuarioId a la publicación
                    publicacion.UsuarioId = usuario.UsuarioId;

                    // Agregar y guardar la publicación en la base de datos
                    _context.Publicaciones.Add(publicacion);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                }
            }

            ViewBag.TipoMascotas = new SelectList(new List<string> { "Perro", "Gato", "Ave", "Otro" });
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
