using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Contrase�a { get; set; }
        public string Tel�fono { get; set; }
    }
}
