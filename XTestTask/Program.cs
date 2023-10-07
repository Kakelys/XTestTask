using XTestTask.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepository(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
