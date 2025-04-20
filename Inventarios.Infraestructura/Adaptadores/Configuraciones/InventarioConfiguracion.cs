

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inventarios.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace Inventarios.Infraestructura.Adaptadores.Configuraciones
{
    [ExcludeFromCodeCoverage]
    public class InventarioConfiguracion : IEntityTypeConfiguration<Inventario>
    {
        public void Configure(EntityTypeBuilder<Inventario> builder)
        {
            builder.ToTable("tbl_inventario");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

            builder.Property(x => x.CantidadStock)
                .HasColumnName("cantidad_stock")
                .IsRequired();

            builder.Property(x => x.IdProducto)
                .HasColumnName("idproducto")
                .IsRequired();

            builder.Property(x => x.FechaCreacion)
                .HasColumnName("fecharegistro")
                .HasColumnType("timestamp(6)")
                .IsRequired();

            builder.Property(x => x.FechaModificacion)
                .HasColumnName("fechaactualizacion")
                .HasColumnType("timestamp(6)")
                .IsRequired(false);

        }
    }
}
