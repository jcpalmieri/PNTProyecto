using System.ComponentModel.DataAnnotations;
namespace PNTProyecto.Models
{
    public class Publicacion
 {
    public int nroPublicacion { get; set; }
    public string NombreMascota { get; set; }
    public string Imagen { get; set; }
    public string TipoMascota { get; set; } 
    public string Descripcion { get; set; }
    public string Contacto { get; set; }
 }
}
