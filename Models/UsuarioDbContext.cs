using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PNTProyecto.Models;
using System.Collections.Generic;

namespace PNTProyecto.Context
{
    public class UsuarioDbContext : DbContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext>
       options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}