using RetoTiendda.Domain.Entities;

namespace RetoTiendda.Domain.Abstractions;

public interface IOrdenRepository
{
    Task AddAsync(Orden orden, CancellationToken ct);
    Task<Orden?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Orden orden, CancellationToken ct);
}