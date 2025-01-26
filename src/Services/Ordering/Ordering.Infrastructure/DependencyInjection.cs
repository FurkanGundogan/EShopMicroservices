using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        // register sqlserver
        services.AddDbContext<ApplicationDbContext>(opt =>
        opt.UseSqlServer(connectionString));

        // addscope application dbcontext
        return services;
    }
}

