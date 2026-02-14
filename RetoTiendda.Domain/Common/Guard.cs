using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Domain.Common
{
    public static class Guard
    {
        public static void AgainstNullOrWhiteSpace(string? value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{name} no puede estar vacío.");
        }

        public static void AgainstNonPositive(int value, string name)
        {
            if (value <= 0)
                throw new DomainException($"{name} debe ser mayor a 0.");
        }

        public static void AgainstNegative(decimal value, string name)
        {
            if (value < 0)
                throw new DomainException($"{name} no puede ser negativo.");
        }
    }
}
