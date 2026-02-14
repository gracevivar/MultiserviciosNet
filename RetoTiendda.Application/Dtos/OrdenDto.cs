using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.Dtos
{
    public sealed record OrdenItemDto(string ProductoId, int Cantidad, decimal PrecioUnitario, decimal LineTotal);

    public sealed record OrdenDto(
        Guid Id,
        string ClienteId,
        string Estado,
        string Moneda,
        decimal Total,
        IReadOnlyList<OrdenItemDto> Items
    );
}
