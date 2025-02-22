using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        // register sqlserver
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            opt.UseSqlServer(connectionString);
        });

        // addscope application dbcontext 235
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
/// added this intreface to actual dbcontext
/// this registration enables the injection of IApplicationDbContext interface throught to applicationLayer 
/// where we implement actual business logic which is createOrderHandler

        return services;
    }
}

