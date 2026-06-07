using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configs
{
    public class FacturaDetalleConfiguration : IEntityTypeConfiguration<FacturaDetalle>
    {
        public void Configure(EntityTypeBuilder<FacturaDetalle> builder)
        {
            builder.ToTable("FacturaDetalles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Cantidad)
                .IsRequired();

            builder.Property(x => x.PrecioUnitario)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Subtotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Factura)
                .WithMany(f => f.Detalles)
                .HasForeignKey(x => x.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Producto)
                .WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
