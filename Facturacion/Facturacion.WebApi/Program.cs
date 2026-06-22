using Facturacion.Application.Interfaces;
using Facturacion.Application.UseCases;
using Facturacion.Application.Validators.Cliente;
using Facturacion.Domain.Interfaces;
using Facturacion.Infrastructure.Context;
using Facturacion.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Facturacion.Application.DependecyInjection;
using Facturacion.Infrastructure.DependencyInjection;
using Facturacion.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa solo el token JWT (sin el prefijo 'Bearer')"
    });

    c.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

//Comentado por uso de metodos de extensión por proyecto
////Conexion a base de datos
//builder.Services.AddDbContext<FacturacionContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////Repositories
//builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
//builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
//builder.Services.AddScoped<IFacturaDetalleRepository, FacturaDetalleRepository>();
//builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

////Services
//builder.Services.AddScoped<IClienteService, ClienteService>();
//builder.Services.AddScoped<IFacturaService, FacturaService>();
//builder.Services.AddScoped<IFacturaDetalleService, FacturaDetalleService>();
//builder.Services.AddScoped<IProductoService, ProductoService>();

//builder.Services.AddValidatorsFromAssemblyContaining<CreateClienteValidatorDto>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Este middleware se ejcutar antes de los controllers
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication(); //ESTE SIEMPRE DEBE ESTAR ANTES DEL UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();


//Comando para migraciones
/*
 Les comparto los comandos para ejecutar migraciones.

--A�adir migraci�n indicando el proyecto de infrastructure ya sea Local, Client o Admin.
add-migration IsActiveClient -Project Local.Infrastructure -StartupProject Local.WebApi

--Ejecutar la migraci�n al proyecto indicando el proyecto de infrastructure ya sea Local, Client o Admin.
Update-Database -Project Local.Infrastructure -StartupProject Local.WebApi
 
--Remover la ultima migraci�n creada en el proyecto indicando el proyecto de infrastructure ya sea Local, Client o Admin.
remove-migration -Project Local.Infrastructure -StartupProject Local.WebApi
 
--Recuerden siempre colocar en -StartupProject el proyecto WebApi donde necesiten ejecutar la migraci�n

 */