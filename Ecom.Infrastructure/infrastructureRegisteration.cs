using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure;

public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IImageManagementService, ImageManagementService>();
        services.AddSingleton<IFileProvider>(
new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        services.AddDbContext<AppDbContext>(op =>
        {
            op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
        });
        return services;
    }
}
