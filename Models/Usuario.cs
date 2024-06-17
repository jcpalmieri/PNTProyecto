using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Teléfono { get; set; }
    }
}
