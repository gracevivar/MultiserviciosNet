using RetoTiendda.Application.Dtos;
using RetoTiendda.Application.Mappings;
using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Common;
using RetoTiendda.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.UseCases.CrearItem
{
    public class CrearOrdenItemUseCase
    {
        private readonly IOrdenRepository ordenRepository;
        public CrearOrdenItemUseCase(IOrdenRepository ordenRepository) => ordenRepository = ordenRepository;
        public async Task<OrdenDto> ExecuteAsync(AddOrdenItemCommand cmd, CancellationToken ct)
        {
            var orden = await ordenRepository.GetByIdAsync(cmd.OrdenId, ct)
                ?? throw new DomainException("Orden no encontrada.");

            orden.AddItem(cmd.ProductoId, cmd.Cantidad, new Moneda(cmd.PrecioUnitario, orden.Currency));

            await ordenRepository.UpdateAsync(orden, ct);

            return orden.ToDto();
        }
    }
}
