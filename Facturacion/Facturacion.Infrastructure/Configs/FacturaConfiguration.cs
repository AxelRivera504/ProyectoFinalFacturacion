using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configs
{
    public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> builder)
        {
            builder.ToTable("Facturas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Total)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Estado)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("pendiente");

            builder.Property(x => x.FechaFactura)
                .IsRequired();

            builder.HasOne(x => x.Cliente)
                .WithMany()
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
