using System.Collections.Concurrent;
using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Entities;

namespace RetoTiendda.Infrastructure.Persistence;

public sealed class InMemoryOrdenRepository : IOrdenRepository
{
    private static readonly ConcurrentDictionary<Guid, Orden> Store = new();

    public Task AddAsync(Orden orden, CancellationToken ct)
    {
        Store[orden.Id] = orden;
        return Task.CompletedTask;
    }

    public Task<Orden?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        Store.TryGetValue(id, out var orden);
        return Task.FromResult(orden);
    }

    public Task UpdateAsync(Orden orden, CancellationToken ct)
    {
        Store[orden.Id] = orden;
        return Task.CompletedTask;
    }
}