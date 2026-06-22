using Facturacion.Application.Dtos.FacturaDetalle;
using Facturacion.Application.Exceptions;
using Facturacion.Application.Extensions;
using Facturacion.Application.Interfaces;
using Facturacion.Application.Validators.FacturaDetalle;
using Facturacion.Domain.Interfaces;

namespace Facturacion.Application.UseCases
{
    public class FacturaDetalleService : IFacturaDetalleService
    {
        private readonly IFacturaDetalleRepository _detalleRepository;
        private readonly IFacturaRepository _facturaRepository;

        public FacturaDetalleService(IFacturaDetalleRepository detalleRepository, IFacturaRepository facturaRepository)
        {
            _detalleRepository = detalleRepository;
            _facturaRepository = facturaRepository;
        }

        public async Task<List<FacturaDetalleDto>> GetAllAsync()
        {
            var detalles = await _detalleRepository.GetAllAsync();
            return detalles.Select(d => d.ToDto()).ToList();
        }

        public async Task<List<FacturaDetalleDto>> GetByFacturaIdAsync(int facturaId)
        {
            var factura = await _facturaRepository.GetByIdAsync(facturaId);
            if (factura is null)
                throw new NotFoundException("Factura", facturaId);

            var detalles = await _detalleRepository.GetByFacturaIdAsync(facturaId);
            return detalles.Select(d => d.ToDto()).ToList();
        }

        public async Task<FacturaDetalleDto> GetByIdAsync(int id)
        {
            var detalle = await _detalleRepository.GetByIdAsync(id);
            if (detalle is null)
                throw new NotFoundException("FacturaDetalle", id);
            return detalle.ToDto();
        }

        public async Task<FacturaDetalleDto> CreateAsync(int facturaId, CreateFacturaDetalleDto dto)
        {
            var factura = await _facturaRepository.GetByIdAsync(facturaId);
            if (factura is null)
                throw new NotFoundException("Factura", facturaId);

            var validator = new CreateFacturaDetalleValidatorDto();
            var result = validator.Validate(dto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var detalle = dto.ToEntity();
            detalle.FacturaId = facturaId;
            var creado = await _detalleRepository.CreateAsync(detalle);
            await RecalcularTotalAsync(facturaId);
            return creado.ToDto();
        }

        public async Task<FacturaDetalleDto> UpdateAsync(int id, UpdateFacturaDetalleDto dto)
        {
            var validator = new UpdateFacturaDetalleValidatorDto();
            var result = validator.Validate(dto);
            if (!result.IsValid)
                throw new BusinessException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            var detalleExistente = await _detalleRepository.GetByIdAsync(id);
            if (detalleExistente is null)
                throw new NotFoundException("FacturaDetalle", id);

            var actualizado = await _detalleRepository.UpdateAsync(id, dto.ToEntity());
            await RecalcularTotalAsync(detalleExistente.FacturaId);
            return actualizado.ToDto();
        }

        public async Task DeleteAsync(int id)
        {
            var detalle = await _detalleRepository.GetByIdAsync(id);
            if (detalle is null)
                throw new NotFoundException("FacturaDetalle", id);

            int facturaId = detalle.FacturaId;
            await _detalleRepository.DeleteAsync(id);
            await RecalcularTotalAsync(facturaId);
        }

        private async Task RecalcularTotalAsync(int facturaId)
        {
            var detalles = await _detalleRepository.GetByFacturaIdAsync(facturaId);
            var total = detalles.Sum(d => d.Subtotal);
            await _facturaRepository.RecalcularTotalAsync(facturaId, total);
        }
    }
}
