using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

/// Add services to built-in dependecy injection container

builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssemblies(assembly);
    // add behaviors to mediatr request lifecycle
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts => 
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);

}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository,BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

/// Build the web application
var app = builder.Build();

/// Configure the HTTP request pipeline

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
