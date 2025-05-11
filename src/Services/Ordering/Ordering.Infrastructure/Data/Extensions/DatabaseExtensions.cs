using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure.Data.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitalizeDatabaseAsync(this WebApplication app)
    {

        /// this make same job as update-database command
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        //await SeedOrderandItemsAsync(context);
    }

    public static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        bool anyExistingCustomers = await context.Customers.AnyAsync();
        if (!anyExistingCustomers) 
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProductAsync(ApplicationDbContext context)
    {
        bool anyExistingProducts = await context.Products.AnyAsync();
        if (!anyExistingProducts)
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedOrderandItemsAsync(ApplicationDbContext context)
    {
        bool anyExistingOrders = await context.Orders.AnyAsync();
        if (!anyExistingOrders)
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }
}
