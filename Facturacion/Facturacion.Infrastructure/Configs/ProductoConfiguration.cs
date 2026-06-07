using Facturacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Facturacion.Infrastructure.Configs
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Productos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Categoria)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Precio)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Stock)
                .IsRequired();
        }
    }
}
