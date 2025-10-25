using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ecom.Infrastructure;

public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services)
    {
        // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        //services.AddTransient
        //services.AddScoped
        //services.AddSingleton
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}
