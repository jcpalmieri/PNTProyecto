using Microsoft.EntityFrameworkCore;

namespace PNTProyecto.Models

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}