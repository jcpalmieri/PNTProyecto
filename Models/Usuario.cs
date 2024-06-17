using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Contraseña { get; set; }

        [Required]
        public string Teléfono { get; set; }
    }
}
