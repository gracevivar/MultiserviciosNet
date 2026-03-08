using Microsoft.Extensions.DependencyInjection;
using RetoTiendda.Domain.Abstractions;
using RetoTiendda.Infrastructure.Persistence;

namespace RetoTiendda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOrdenRepository, InMemoryOrdenRepository>();
        return services;
    }
}