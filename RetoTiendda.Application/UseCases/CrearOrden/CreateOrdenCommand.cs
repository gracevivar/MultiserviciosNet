using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application.UseCases.CrearOrden
{

    public sealed record CreateOrdenCommand(string ClienteId, string Moneda);
}
