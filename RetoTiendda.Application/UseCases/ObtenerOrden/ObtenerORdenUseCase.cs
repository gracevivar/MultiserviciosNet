using RetoTiendda.Application.Dtos;
using RetoTiendda.Application.Mappings;
using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Common;

namespace RetoTiendda.Application.UseCases.ObtenerOrden;

public sealed class ObtenerORdenUseCase
{
    private readonly IOrdenRepository _repo;

    public ObtenerORdenUseCase(IOrdenRepository repo) => _repo = repo;

    public async Task<OrdenDto> ExecuteAsync(Guid ordenId, CancellationToken ct)
    {
        var orden = await _repo.GetByIdAsync(ordenId, ct)
            ?? throw new DomainException("Orden no encontrada.");

        return orden.ToDto();
    }
}