<<<<<<< HEAD
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
        public string Contraseña { get; set; }
        [Required]
        public string Teléfono { get; set; }
    }
=======
ï»¿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PNTProyecto.Models
{
	public Usuario()
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string email { get; set; }
        public string nombreUsuario {  get; set; }
	    public string nroTelefono { get; set; }
        public string Imagen { get; set; }
}
>>>>>>> ddf1d166cba508d8e71f9a034f629cabb8c3e24f
}
