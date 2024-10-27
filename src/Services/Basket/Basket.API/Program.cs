var builder = WebApplication.CreateBuilder(args);

/// Add services to built-in dependecy injection container

/// Build the web application
var app = builder.Build();

/// Configure the HTTP request pipeline

app.MapGet("/", () => "Hello World!");

app.Run();
