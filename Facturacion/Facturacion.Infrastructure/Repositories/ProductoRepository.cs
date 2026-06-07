using Facturacion.Domain.Entities;
using Facturacion.Domain.Interfaces;
using Facturacion.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Facturacion.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly FacturacionContext _context;

        public ProductoRepository(FacturacionContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
            => await _context.Productos.ToListAsync();

        public async Task<Producto> GetByIdAsync(int id)
            => await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Producto>> SearchAsync(string nombre)
            => await _context.Productos.Where(p => p.Nombre.Contains(nombre)).ToListAsync();

        public async Task<Producto> CreateAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateAsync(int id, Producto producto)
        {
            var productoExistente = await GetByIdAsync(id);
            if (productoExistente is not null)
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Categoria = producto.Categoria;
                productoExistente.Precio = producto.Precio;
                productoExistente.Stock = producto.Stock;
                await _context.SaveChangesAsync();
            }
            return productoExistente;
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await GetByIdAsync(id);
            if (producto is not null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
