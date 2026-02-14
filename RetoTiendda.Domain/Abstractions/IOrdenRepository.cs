using RetoTiendda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Domain.Abstractions
{
    public interface IOrdenRepository
    {
        /*public async Task<object?> GetByIdAsync(Guid ordenId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }*/

        Task AddAsync(Orden orden, CancellationToken ct);
        Task<Orden?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(Orden orden, CancellationToken ct);
    }
}
