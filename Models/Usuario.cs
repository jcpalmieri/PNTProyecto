using System.ComponentModel.DataAnnotations.Schema;
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
}
