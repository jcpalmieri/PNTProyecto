using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNTProyecto.Models;
using System.Threading.Tasks;

namespace PNTProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("UsuarioId,NombreUsuario,Email,Password,Telefono")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,NombreUsuario,Email,Password,Telefono")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el usuario actual de la base de datos
                    var existingUsuario = await _context.Usuarios.FindAsync(usuario.UsuarioId);

                    if (existingUsuario == null)
                    {
                        return NotFound();
                    }

                    // Si se proporcionó una nueva contraseña, actualizarla
                    if (!string.IsNullOrEmpty(usuario.Password))
                    {
                        // Aquí podrías aplicar hashing u otros métodos de seguridad antes de guardar
                        existingUsuario.Password = usuario.Password;
                    }

                    // Actualizar los demás campos
                    existingUsuario.NombreUsuario = usuario.NombreUsuario;
                    existingUsuario.Email = usuario.Email;
                    existingUsuario.Telefono = usuario.Telefono;

                    // Guardar cambios
                    _context.Update(existingUsuario);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cambios guardados con éxito.";

                    // Redirigir al inicio de sesión
                    return RedirectToAction("Login", "Account", new { area = "", returnUrl = "" });
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

            // Si llegamos aquí, significa que hay errores de validación, así que retornamos la vista con el usuario
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

            // esto seria un objeto respuesta.
            var responseObj = new
            {
                usuario.UsuarioId,
                usuario.Email,
                usuario.Telefono
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