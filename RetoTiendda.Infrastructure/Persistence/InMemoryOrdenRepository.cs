using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Infrastructure.Persistence
{
    public class InMemoryOrdenRepository:IOrdenRepository
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
}
