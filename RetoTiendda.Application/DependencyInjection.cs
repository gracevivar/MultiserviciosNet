using Microsoft.Extensions.DependencyInjection;
using RetoTiendda.Application.UseCases.CrearItem;
using RetoTiendda.Application.UseCases.CrearOrden;
using RetoTiendda.Application.UseCases.ObtenerOrden;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetoTiendda.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateOrdenUseCase>();
            services.AddScoped<CrearOrdenItemUseCase>();
            services.AddScoped<ObtenerORdenUseCase>();
            return services;
        }
    }
}
