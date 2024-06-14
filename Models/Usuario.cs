using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Contrase�a { get; set; }
        [Required]
        public string Tel�fono { get; set; }
    }
}
