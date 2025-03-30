using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

/// Add services to built-in dependecy injection container

// Application Services
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssemblies(assembly);
    // add behaviors to mediatr request lifecycle
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Data Services
builder.Services.AddMarten(opts => 
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);

}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository,BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
/// Decorate changes repository object and its decorated version in CachedBasketRepository class
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
    // opt.InstanceName="Basket";
});


/* 
 * Old version of injecting CachedBasketRepository
builder.Services.AddScoped<IBasketRepository>(provider => { 
    var basketRepository = provider.GetRequiredService<IBasketRepository>();
    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
});
*/

// Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt => {
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() => 
{ 
    var handler = new HttpClientHandler()
    { 
        // Development purpose
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

// Async comm Services
builder.Services.AddMessageBroker(builder.Configuration);
/// this service is a publisher. so it does not send assembly parameter to AddMessageBroker


// Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);



/// Build the web application
var app = builder.Build();

/// Configure the HTTP request pipeline

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
