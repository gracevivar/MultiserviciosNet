using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Factories;

namespace RetoTiendda.Application.UseCases.CrearOrden;

public sealed class CreateOrdenUseCase
{
    private readonly IOrdenRepository _repo;

    public CreateOrdenUseCase(IOrdenRepository repo) => _repo = repo;

    public async Task<Guid> ExecuteAsync(CreateOrdenCommand cmd, CancellationToken ct)
    {
        var orden = OrdenFactory.CreateDraft(cmd.ClienteId, cmd.Moneda);
        await _repo.AddAsync(orden, ct);
        return orden.Id;
    }
}