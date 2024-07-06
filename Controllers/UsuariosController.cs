using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNTProyecto.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace PNTProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;  // Agrega esta línea

        public UsuariosController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;  // Agrega esta línea
            ViewBag.PageTitle = "Editar información";

        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,NombreUsuario,Email,Password,Telefono,ImagenPerfilFile")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (usuario.ImagenPerfilFile != null && usuario.ImagenPerfilFile.Length > 0)
                {
                    var extension = Path.GetExtension(usuario.ImagenPerfilFile.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        var fileName = Path.GetFileNameWithoutExtension(usuario.ImagenPerfilFile.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "profile_images", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await usuario.ImagenPerfilFile.CopyToAsync(stream);
                        }

                        usuario.ImagenPerfil = $"/profile_images/{uniqueFileName}";
                    }
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Redirigir al usuario a la vista de login con los datos insertados
                return RedirectToAction("Login", "Account", new { email = usuario.Email, password = usuario.Password });
            }
            return View(usuario);
        }



        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,NombreUsuario,Email,Password,Telefono,ImagenPerfilFile")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUsuario = await _context.Usuarios.FindAsync(usuario.UsuarioId);
                    if (existingUsuario == null)
                    {
                        return NotFound();
                    }

                    if (usuario.ImagenPerfilFile != null && usuario.ImagenPerfilFile.Length > 0)
                    {
                        var extension = Path.GetExtension(usuario.ImagenPerfilFile.FileName).ToLower();
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            var fileName = Path.GetFileNameWithoutExtension(usuario.ImagenPerfilFile.FileName);
                            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "profile_images", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await usuario.ImagenPerfilFile.CopyToAsync(stream);
                            }

                            existingUsuario.ImagenPerfil = $"/profile_images/{uniqueFileName}";
                        }
                    }

                    existingUsuario.NombreUsuario = usuario.NombreUsuario;
                    existingUsuario.Email = usuario.Email;
                    existingUsuario.Telefono = usuario.Telefono;

                    if (!string.IsNullOrEmpty(usuario.Password))
                    {
                        existingUsuario.Password = usuario.Password;
                    }

                    _context.Update(existingUsuario);
                    await _context.SaveChangesAsync();

                    // TempData["SuccessMessage"] = "Cambios guardados con éxito.";
                    return RedirectToAction("Index", "Perfil", new { id = usuario.UsuarioId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(usuario);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpGet("Usuarios/DetailsByEmailOrUsername/{emailOrUsername}")]
        public async Task<IActionResult> DetailsByEmailOrUsername(string emailOrUsername)
        {
            if (string.IsNullOrEmpty(emailOrUsername))
            {
                return BadRequest("Email or username is required.");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == emailOrUsername || u.NombreUsuario == emailOrUsername);

            if (usuario == null)
            {
                return NotFound();
            }

            var responseObj = new
            {
                usuario.UsuarioId,
                usuario.Email,
                usuario.Telefono,
                imagenPerfil = usuario.ImagenPerfil 
            };

            return Ok(responseObj);
        }


        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}