using Facturacion.Application.Dtos.Factura;
using Facturacion.Application.Exceptions;
using Facturacion.Application.Extensions;
using Facturacion.Application.Interfaces;
using Facturacion.Application.Validators.Factura;
using Facturacion.Domain.Interfaces;

namespace Facturacion.Application.UseCases
{
    public class FacturaService : IFacturaService
    {
        private static readonly string[] EstadosValidos = { "pendiente", "pagada", "anulada" };

        private readonly IFacturaRepository _facturaRepository;
        public FacturaService(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        public async Task<List<FacturaDto>> GetAllAsync()
        {
            var facturas = await _facturaRepository.GetAllAsync();
            return facturas.Select(f => f.ToDto()).ToList();
        }

        public async Task<FacturaDto> GetByIdAsync(int id)
        {
            var factura = await _facturaRepository.GetByIdAsync(id);
            if (factura is null)
                throw new NotFoundException("Factura", id);
            return factura.ToDto();
        }

        public async Task<List<FacturaDto>> GetByClienteIdAsync(int clienteId)
        {
            var facturas = await _facturaRepository.GetByClienteIdAsync(clienteId);
            return facturas.Select(f => f.ToDto()).ToList();
        }

        public async Task<List<FacturaDto>> GetByEstadoAsync(string estado)
        {
            if (!EstadosValidos.Contains(estado?.ToLower()))
                throw new BusinessException("El estado debe ser: pendiente, pagada o anulada");

            var facturas = await _facturaRepository.GetByEstadoAsync(estado);
            return facturas.Select(f => f.ToDto()).ToList();
        }

        public async Task<FacturaDto> CreateAsync(CreateFacturaDto facturaDto)
        {
            var validator = new CreateFacturaValidatorDto();
            var result = validator.Validate(facturaDto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var factura = facturaDto.ToEntity();
            factura.Total = factura.Detalles.Sum(d => d.Subtotal);
            var facturaCreada = await _facturaRepository.CreateAsync(factura);
            var facturaCompleta = await _facturaRepository.GetByIdAsync(facturaCreada.Id);
            return facturaCompleta.ToDto();
        }

        public async Task<FacturaDto> UpdateAsync(int id, UpdateFacturaDto facturaDto)
        {
            var validator = new UpdateFacturaValidatorDto();
            var result = validator.Validate(facturaDto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var existente = await _facturaRepository.GetByIdAsync(id);
            if (existente is null)
                throw new NotFoundException("Factura", id);

            var factura = await _facturaRepository.UpdateAsync(id, facturaDto.ToEntity());
            return factura.ToDto();
        }

        public async Task UpdateEstadoAsync(int id, string estado)
        {
            if (!EstadosValidos.Contains(estado?.ToLower()))
                throw new BusinessException("El estado debe ser: pendiente, pagada o anulada");

            var existente = await _facturaRepository.GetByIdAsync(id);
            if (existente is null)
                throw new NotFoundException("Factura", id);

            await _facturaRepository.UpdateEstadoAsync(id, estado);
        }

        public async Task DeleteAsync(int id)
        {
            var existente = await _facturaRepository.GetByIdAsync(id);
            if (existente is null)
                throw new NotFoundException("Factura", id);

            await _facturaRepository.DeleteAsync(id);
        }
    }
}
