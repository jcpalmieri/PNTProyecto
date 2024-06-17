using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Publicacion
    {
        [Key]
        public int nroPublicacion { get; set; }
        [Required(ErrorMessage = "El nombre de la mascota es requerido.")]
        public string nombreMascota { get; set; }
        [Required(ErrorMessage = "La descripción es requerida.")]
        public string descripcion { get; set; }

        public string imagen { get; set; }
    }

}
