using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

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
        public string Password { get; set; }

        [Required]
        public string Telefono { get; set; }

        public string? ImagenPerfil { get; set; }  // Campo para la ruta de la imagen de perfil

        [NotMapped]
        public IFormFile ImagenPerfilFile { get; set; }  // Campo para la subida de la imagen de perfil
    }
}