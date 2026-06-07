using Facturacion.Domain.Entities;
using Facturacion.Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Context
{
    public class FacturacionContext : DbContext
    {
        public FacturacionContext(DbContextOptions<FacturacionContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteConfiguration).Assembly);
        }
    }
}
