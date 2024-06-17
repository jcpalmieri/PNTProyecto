using System.ComponentModel.DataAnnotations;

namespace PNTProyecto.Models
{
    public class Publicacion
    {
        [Key]
        public int nroPublicacion { get; set; }
        
        public string nombreMascota { get; set; }
        
        public string descripcion { get; set; }

        public string imagen { get; set; }
    }

}
