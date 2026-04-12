namespace RetoTienda.Contracts.Events;

public sealed record OrderCreated(
    Guid OrderId,
    string CustomerId,
    DateTime CreatedAtUtc
);