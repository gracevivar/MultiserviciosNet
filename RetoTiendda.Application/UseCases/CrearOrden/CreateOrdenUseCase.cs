using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.UseCases.CrearOrden
{
    public class CreateOrdenUseCase
    {
        private readonly IOrdenRepository _repo;

        public CreateOrdenUseCase(IOrdenRepository repo) => _repo = repo;

        public async Task<Guid> ExecuteAsync(CreateOrdenCommand cmd, CancellationToken ct)
        {
            var order = OrdenFactory.CreateDraft(cmd.ClienteId, cmd.Moneda);
            await _repo.AddAsync(order, ct);
            return order.Id;
        }
    }
}
