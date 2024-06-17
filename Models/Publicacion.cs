using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Publicacion
    {
        [Key]
        public int nroPublicacion { get; set; }

        public string nombreMascota { get; set; }

        public string descripcion { get; set; }

        [DataType(DataType.ImageUrl)]
        public string imagen { get; set; }
    }
}
