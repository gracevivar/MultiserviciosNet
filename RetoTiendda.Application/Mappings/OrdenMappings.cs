using RetoTiendda.Application.Dtos;
using RetoTiendda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.Mappings
{
    public static class OrdenMappings
    {
        public static OrdenDto ToDto(this Orden orden)
        => new(
            orden.Id,
            orden.ClienteId,
            orden.Estado.ToString(),
            orden.Currency,
            orden.Total.Amount,
            orden.Items.Select(i => new OrdenItemDto(
                i.ProductoId,
                i.Cantidad,
                i.PrecioUnitario.Amount,
                i.Total.Amount
            )).ToList()
        );
    }
}
