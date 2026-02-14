using RetoTiendda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Domain.Factories
{
    public class OrdenFactory
    {
        public static Orden CreateDraft(string customerId, string currency)
        => new Orden(Guid.NewGuid(), customerId, DateTime.UtcNow, currency);
    }
}
