using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PNTProyecto.Models
{
    public class Publicacion
 {
     [Key]
     public int nroPublicacion { get; set; }
     public string NombreMascota { get; set; }
     public string Imagen { get; set; }
     public string TipoMascota { get; set; } 
     public string Descripcion { get; set; }
     public string Contacto { get; set; }
    public string? UsuarioInteresado { get; set; }
    public int UsuarioId { get; set; }
        [NotMapped]
        public IFormFile? ImagenFile { get; set; }

        // Propiedad calculada que no se almacena en la base de datos
        [NotMapped]
        public int CantidadUsuariosInteresados
        {
            get
            {
                if (string.IsNullOrEmpty(UsuarioInteresado))
                {
                    return 0;
                }

                // Contar la cantidad de IDs separados por comas
                var ids = UsuarioInteresado.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return ids.Length;
            }
        }
    }

}
