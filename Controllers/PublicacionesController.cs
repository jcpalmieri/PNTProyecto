using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using PNTProyecto.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
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
            return View(await _context.Publicaciones.ToListAsync());
        }

        public async Task<IActionResult> MisPublicaciones(int usuarioId)
        {
            var publicaciones = await _context.Publicaciones
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
            return View("Index", publicaciones);
        }

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

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

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
                var userName = User.Identity.Name;
                var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

                if (usuario != null)
                {
                    publicacion.UsuarioId = usuario.UsuarioId;
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

            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View(publicacion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("nroPublicacion,NombreMascota,Descripcion,TipoMascota,Contacto,Imagen")] Publicacion publicacion)
        {
            if (id != publicacion.nroPublicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userName = User.Identity.Name;
                    var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

                    if (usuario != null)
                    {
                        // Obtener la entidad existente desde la base de datos
                        var existingPublicacion = await _context.Publicaciones.FindAsync(id);

                        // Copiar los valores editables desde el formulario a la entidad existente
                        existingPublicacion.NombreMascota = publicacion.NombreMascota;
                        existingPublicacion.Descripcion = publicacion.Descripcion;
                        existingPublicacion.TipoMascota = publicacion.TipoMascota;
                        existingPublicacion.Contacto = publicacion.Contacto;
                        existingPublicacion.Imagen = publicacion.Imagen;

                        // Guardar los cambios en la base de datos
                        _context.Update(existingPublicacion);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Usuario no encontrado.");
                    }
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
            }

            ViewBag.TipoMascotas = new List<string> { "Perro", "Gato", "Ave", "Pez", "Roedor" };
            return View(publicacion);
        }



        // GET: Publicaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publicacion = await _context.Publicaciones.FirstOrDefaultAsync(m => m.nroPublicacion == id);

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

        [HttpPost]
        public async Task<IActionResult> ToggleMeGusta([FromBody] ToggleMeGustaRequest request)
        {
            var publicacionId = request.PublicacionId;
            var userId = HttpContext.Session.GetInt32("UserId");

            _logger.LogInformation("ToggleMeGusta action called for PublicacionId: {publicacionId} by UserId: {userId}", publicacionId, userId);

            if (userId == null)
            {
                return Json(new { success = false, message = "Usuario no autenticado." });
            }

            var usuario = await _context.Usuarios.FindAsync(userId.Value);

            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            var publicacion = await _context.Publicaciones.FindAsync(publicacionId);

            if (publicacion == null)
            {
                return Json(new { success = false, message = "Publicación no encontrada." });
            }

            var usuariosInteresados = string.IsNullOrEmpty(publicacion.UsuarioInteresado)
                ? new List<string>()
                : publicacion.UsuarioInteresado.Split(',').ToList();

            if (usuariosInteresados.Contains(usuario.UsuarioId.ToString()))
            {
                usuariosInteresados.Remove(usuario.UsuarioId.ToString());
            }
            else
            {
                usuariosInteresados.Add(usuario.UsuarioId.ToString());
            }

            publicacion.UsuarioInteresado = string.Join(",", usuariosInteresados);

            try
            {
                _context.Update(publicacion);
                await _context.SaveChangesAsync();

                var contacto = publicacion.Contacto;

                return Json(new { success = true, message = "Me Gusta toggled correctamente.", contacto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar los cambios.");
                return Json(new { success = false, message = "Error al guardar los cambios." });
            }
        }


        public class ToggleMeGustaRequest
        {
            public int PublicacionId { get; set; }
        }

    }
}
