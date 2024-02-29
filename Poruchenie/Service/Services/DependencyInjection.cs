using Poruchenie.Service.Services;
using Poruchenie.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EcoLink.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add repositories
        services.AddScoped(typeof(IYurdikService), typeof(YurdikService));
        services.AddScoped(typeof(IJismoniyService), typeof(JismoniyService));

        return services;
    }
}

