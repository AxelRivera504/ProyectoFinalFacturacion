using Facturacion.Application.Dtos.Producto;
using Facturacion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _productoService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _productoService.GetByIdAsync(id));

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string nombre)
            => Ok(await _productoService.SearchAsync(nombre));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductoDto createProductoDto)
        {
            var producto = await _productoService.CreateAsync(createProductoDto);
            return StatusCode(201, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductoDto updateProductoDto)
        {
            await _productoService.UpdateAsync(id, updateProductoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
