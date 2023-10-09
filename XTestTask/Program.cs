using Microsoft.AspNetCore.SignalR;
using XTestTask.Data.Repository;
using XTestTask.Hubs;
using XTestTask.Hubs.Filters;
using XTestTask.Middlewares.ExceptionMiddleware;
using XTestTask.Services;
using XTestTask.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(opts => 
{
    opts.AddFilter<ExceptionFilter>();
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddRepository(builder.Configuration);

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatHubService, ChatHubService>();

builder.Services.AddSingleton<ExceptionFilter>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "XTestTask", Version = "v1" });
    c.AddSignalRSwaggerGen();
});


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<AccountHub>("/hubs/account");

app.Run();
