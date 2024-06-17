using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PNTProyecto.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PNTProyecto.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<PublicacionesController> _logger;

        public PublicacionesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, ILogger<PublicacionesController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // GET: Publicaciones
        public async Task<IActionResult> Index()
        {
            var publicaciones = await _context.Publicaciones.ToListAsync();
            _logger.LogInformation("Number of publicaciones: {Count}", publicaciones.Count);
            return View(publicaciones);
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
            return View();
        }

        // POST: Publicaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("nroPublicacion,nombreMascota,descripcion,ImagenFile")] Publicacion publicacion)
        {
            if (ModelState.IsValid)
            {
                if (publicacion.ImagenFile != null && publicacion.ImagenFile.Length > 0)
                {
                    // Ruta relativa dentro de wwwroot/imagenes
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "imagenes");

                    // Nombre único para evitar conflictos
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(publicacion.ImagenFile.FileName);

                    // Ruta completa donde se guardará el archivo
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guardar el archivo en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await publicacion.ImagenFile.CopyToAsync(stream);
                    }

                    // Guardar la ruta de la imagen en la entidad Publicacion
                    publicacion.Imagen = "/imagenes/" + uniqueFileName;
                }

                _context.Add(publicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("nroPublicacion,nombreMascota,descripcion,Imagen")] Publicacion publicacion)
        {
            if (id != publicacion.nroPublicacion)
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
            if (publicacion != null)
            {
                _context.Publicaciones.Remove(publicacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicaciones.Any(e => e.nroPublicacion == id);
        }
    }
}

