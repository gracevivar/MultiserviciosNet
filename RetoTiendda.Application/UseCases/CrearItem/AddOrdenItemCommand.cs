using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.UseCases.CrearItem
{
    public sealed record AddOrdenItemCommand(
        Guid OrdenId,
        string ProductoId,
        int Cantidad,
        decimal PrecioUnitario
    );
    
}
