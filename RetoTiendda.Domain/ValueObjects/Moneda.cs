using RetoTiendda.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Domain.ValueObjects
{
    public class Moneda
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Moneda(decimal amount, string currency)
        {
            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero); ;
            Currency = currency;
        }
        public static Moneda operator +(Moneda a, Moneda b)
        {
            if (a.Currency != b.Currency)
                throw new DomainException("No se pueden sumar montos con distinta moneda.");
            return new Moneda(a.Amount + b.Amount, a.Currency);
        }

        public static Moneda operator *(Moneda a, int multiplier)
        {
            Guard.AgainstNonPositive(multiplier, nameof(multiplier));
            return new Moneda(a.Amount * multiplier, a.Currency);
        }
    }
}
