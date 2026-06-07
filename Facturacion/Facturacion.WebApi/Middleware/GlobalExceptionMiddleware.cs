using Facturacion.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Facturacion.WebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                await EscribirRespuesta(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex.Message);
                await EscribirRespuesta(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error no controlado");
                await EscribirRespuesta(context, HttpStatusCode.InternalServerError, "Ocurrio un error interno en el servidor");
            }
        }

        private static async Task EscribirRespuesta(HttpContext context, HttpStatusCode statusCode, string mensaje)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                StatusCode = (int)statusCode,
                mensaje
            };

            var json = JsonSerializer.Serialize(respuesta, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
