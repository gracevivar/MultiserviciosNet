using MassTransit;
using RetoTiendda.Application;
using RetoTiendda.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Registrar capas
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "rabbitmq";
        var user = builder.Configuration["RabbitMq:User"] ?? "guest";
        var pass = builder.Configuration["RabbitMq:Pass"] ?? "guest";

        cfg.Host(host, "/", h =>
        {
            h.Username(user);
            h.Password(pass);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();