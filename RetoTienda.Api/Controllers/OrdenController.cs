using Microsoft.AspNetCore.Mvc;
using RetoTiendda.Application.UseCases.CrearItem;
using RetoTiendda.Application.UseCases.CrearOrden;
using RetoTiendda.Application.UseCases.ObtenerOrden;
using RetoTiendda.Domain.Common;

namespace RetoTienda.Api.Controllers;

[ApiController]
[Route("api/ordenes")]
public sealed class OrdenController : ControllerBase
{
    private readonly CreateOrdenUseCase _createOrden;
    private readonly CrearOrdenItemUseCase _addItem;
    private readonly ObtenerORdenUseCase _getOrden;

    public OrdenController(
        CreateOrdenUseCase createOrden,
        CrearOrdenItemUseCase addItem,
        ObtenerORdenUseCase getOrden)
    {
        _createOrden = createOrden;
        _addItem = addItem;
        _getOrden = getOrden;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrdenRequest req, CancellationToken ct)
    {
        try
        {
            var id = await _createOrden.ExecuteAsync(new(req.ClienteId, req.Moneda), ct);
            return CreatedAtAction(nameof(GetById), new { orderId = id }, new { ordenId = id });
        }
        catch (DomainException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddItem(Guid orderId, [FromBody] AddItemRequest req, CancellationToken ct)
    {
        try
        {
            var dto = await _addItem.ExecuteAsync(new(orderId, req.ProductoId, req.Cantidad, req.PrecioUnitario), ct);
            return Ok(dto);
        }
        catch (DomainException ex)
        {
            var status = ex.Message.Contains("no encontrada", StringComparison.OrdinalIgnoreCase)
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            return Problem(detail: ex.Message, statusCode: status);
        }
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetById(Guid orderId, CancellationToken ct)
    {
        try
        {
            var dto = await _getOrden.ExecuteAsync(orderId, ct);
            return Ok(dto);
        }
        catch (DomainException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
        }
    }
}

public sealed record CreateOrdenRequest(string ClienteId, string Moneda);
public sealed record AddItemRequest(string ProductoId, int Cantidad, decimal PrecioUnitario);