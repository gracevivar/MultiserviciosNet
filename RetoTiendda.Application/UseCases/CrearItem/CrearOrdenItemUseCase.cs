using RetoTiendda.Application.Dtos;
using RetoTiendda.Application.Mappings;
using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Common;
using RetoTiendda.Domain.ValueObjects;

namespace RetoTiendda.Application.UseCases.CrearItem;

public sealed class CrearOrdenItemUseCase
{
    private readonly IOrdenRepository _ordenRepository;

    public CrearOrdenItemUseCase(IOrdenRepository ordenRepository) => _ordenRepository = ordenRepository;

    public async Task<OrdenDto> ExecuteAsync(AddOrdenItemCommand cmd, CancellationToken ct)
    {
        var orden = await _ordenRepository.GetByIdAsync(cmd.OrdenId, ct)
            ?? throw new DomainException("Orden no encontrada.");

        orden.AddItem(cmd.ProductoId, cmd.Cantidad, new Moneda(cmd.PrecioUnitario, orden.Currency));

        await _ordenRepository.UpdateAsync(orden, ct);

        return orden.ToDto();
    }
}