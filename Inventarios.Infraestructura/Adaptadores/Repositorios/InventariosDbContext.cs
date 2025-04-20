using Microsoft.EntityFrameworkCore;
using Inventarios.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;
using Inventarios.Infraestructura.Adaptadores.Configuraciones;

namespace Inventarios.Infraestructura.Adaptadores.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class InventariosDbContext : DbContext
    {
        public InventariosDbContext(DbContextOptions<InventariosDbContext> options): base(options){ }

        public DbSet<Inventario> Inventarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InventarioConfiguracion());
        }
    }
}
