using Facturacion.Application.Interfaces;
using Facturacion.Application.UseCases;
using Facturacion.Application.Validators.Cliente;
using Facturacion.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.DependecyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<IFacturaDetalleService, FacturaDetalleService>();
            services.AddScoped<IProductoService, ProductoService>();

            services.AddValidatorsFromAssemblyContaining<CreateClienteValidatorDto>();

            return services;
        }
    }
}
