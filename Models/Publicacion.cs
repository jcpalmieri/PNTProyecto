using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Publicacion
    {
        [Key]
        public int nroPublicacion { get; set; }

        public string nombreMascota { get; set; }

        public string descripcion { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImagenFile { get; set; } // Propiedad para capturar el archivo de imagen
    }
}
