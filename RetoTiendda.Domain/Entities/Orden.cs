using RetoTiendda.Domain.Common;
using RetoTiendda.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

public enum OrderStatus { Draft = 1, Confirmed = 2 }

namespace RetoTiendda.Domain.Entities
{
    public class Orden
    {
        private readonly List<ItemOrden> _items = new();

        public Guid Id { get; private set; }
        public string ClienteId { get; private set; } = "";
        public DateTime FechaCreate { get; private set; }
        public OrderStatus Estado { get; private set; }
        public string Currency { get; private set; } = "";
        public IReadOnlyCollection<ItemOrden> Items => _items.AsReadOnly();

        public Moneda Total
        {
            get
            {
                var total = new Moneda(0, Currency);
                foreach (var item in _items) total += item.Total;
                return total;
            }
        }
        private Orden() { }
        internal Orden(Guid id, string clienteId, DateTime fechaCreate, string moneda)
        {
            Guard.AgainstNullOrWhiteSpace(clienteId, nameof(clienteId));
            Guard.AgainstNullOrWhiteSpace(moneda, nameof(moneda));

            Id = id;
            ClienteId = clienteId.Trim();
            FechaCreate = fechaCreate;
            Currency = moneda.Trim().ToUpperInvariant();
            Estado = OrderStatus.Draft;
        }
        public void AddItem(string productoId, int cantidad, Moneda precioUnitario)
        {
            if (Estado != OrderStatus.Draft)
                throw new DomainException("Solo se pueden agregar items a una orden en estado Draft.");

            if (precioUnitario.Currency != Currency)
                throw new DomainException("La moneda del item no coincide con la moneda de la orden.");

            var existing = _items.FirstOrDefault(i => i.ProductoId == productoId.Trim());
            if (existing is not null)
            {
                existing.IncreaseQuantity(cantidad);
                return;
            }

            _items.Add(new ItemOrden(productoId, cantidad, precioUnitario));
        }

        public void Confirm()
        {
            if (_items.Count == 0)
                throw new DomainException("No se puede confirmar una orden sin items.");
            Estado = OrderStatus.Confirmed;
        }
    }
}
