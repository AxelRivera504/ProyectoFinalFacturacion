using Facturacion.Domain.Interfaces;
using Facturacion.Infrastructure.Context;
using Facturacion.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Facturacion.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FacturacionContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            services.AddScoped<IFacturaDetalleRepository, FacturaDetalleRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();

            return services;
        }
    }
}
