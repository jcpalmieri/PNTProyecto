using System;
using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Publicacion
    {
        [Key]
        public int NroPublicacion { get; set; }

        [Required]
        [Display(Name = "Nombre de la Mascota")]
        public string NombreMascota { get; set; }

        [Display(Name = "Imagen")]
        public string Imagen { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Tipo de Mascota")]
        public string TipoMascota { get; set; }

        [Required]
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }
    }
}
