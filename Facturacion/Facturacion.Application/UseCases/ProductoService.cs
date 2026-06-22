using Facturacion.Application.Dtos.Producto;
using Facturacion.Application.Exceptions;
using Facturacion.Application.Extensions;
using Facturacion.Application.Interfaces;
using Facturacion.Application.Validators.Producto;
using Facturacion.Domain.Interfaces;

namespace Facturacion.Application.UseCases
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<List<ProductoDto>> GetAllAsync()
        {
            var productos = await _productoRepository.GetAllAsync();
            return productos.Select(p => p.ToDto()).ToList();
        }

        public async Task<ProductoDto> GetByIdAsync(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto is null)
                throw new NotFoundException("Producto", id);
            return producto.ToDto();
        }

        public async Task<List<ProductoDto>> SearchAsync(string nombre)
        {
            var productos = await _productoRepository.SearchAsync(nombre);
            return productos.Select(p => p.ToDto()).ToList();
        }

        public async Task<ProductoDto> CreateAsync(CreateProductoDto productoDto)
        {
            var validator = new CreateProductoValidatorDto();
            var result = validator.Validate(productoDto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var producto = await _productoRepository.CreateAsync(productoDto.ToEntity());
            return producto.ToDto();
        }

        public async Task<ProductoDto> UpdateAsync(int id, UpdateProductoDto productoDto)
        {
            var validator = new UpdateProductoValidatorDto();
            var result = validator.Validate(productoDto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var existente = await _productoRepository.GetByIdAsync(id);
            if (existente is null)
                throw new NotFoundException("Producto", id);

            var producto = await _productoRepository.UpdateAsync(id, productoDto.ToEntity());
            return producto.ToDto();
        }

        public async Task DeleteAsync(int id)
        {
            var existente = await _productoRepository.GetByIdAsync(id);
            if (existente is null)
                throw new NotFoundException("Producto", id);

            await _productoRepository.DeleteAsync(id);
        }
    }
}
