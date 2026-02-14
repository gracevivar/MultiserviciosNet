using RetoTiendda.Domain.Common;
using RetoTiendda.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Domain.Entities
{
    public sealed class ItemOrden
    {
        public string ProductoId { get; private set; } = "";
        public int Cantidad { get; private set; }
        public Moneda PrecioUnitario { get; private set; }

        public Moneda Total=> PrecioUnitario * Cantidad;

        private ItemOrden() { }

        internal ItemOrden(string productoId, int cantidad, Moneda preciounitario)
        {
            Guard.AgainstNullOrWhiteSpace(productoId, nameof(productoId));
            Guard.AgainstNonPositive(cantidad, nameof(cantidad));

            ProductoId = productoId.Trim();
            Cantidad = cantidad;
            PrecioUnitario = preciounitario;
        }

        internal void IncreaseQuantity(int cantidad)
        {
            Guard.AgainstNonPositive(cantidad, nameof(cantidad));
            Cantidad += cantidad;
        }
    }
}
