using MassTransit;
using RetoTienda.Contracts.Events;

namespace RetoTienda.Worker.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        var m = context.Message;
        Console.WriteLine($"[Worker] OrderCreated => OrderId={m.OrderId}, CustomerId={m.CustomerId}, CreatedAtUtc={m.CreatedAtUtc:o}");
        return Task.CompletedTask;
    }
}