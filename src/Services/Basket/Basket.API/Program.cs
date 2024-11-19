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
/// Build the web application
var app = builder.Build();

/// Configure the HTTP request pipeline

app.MapCarter();

app.Run();
