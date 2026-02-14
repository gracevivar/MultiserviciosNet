using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetoTiendda.Application.UseCases.CrearItem;
using RetoTiendda.Application.UseCases.CrearOrden;
using RetoTiendda.Application.UseCases.ObtenerOrden;
using RetoTiendda.Domain.Common;

namespace RetoTienda.Api.Controllers
{
    public class OrdenController : Controller
    {
        private readonly CreateOrdenUseCase _createOrder;
        private readonly CrearOrdenItemUseCase _addItem;
        private readonly ObtenerORdenUseCase _getOrder;
        public OrdenController(
        CreateOrdenUseCase createOrder,
        CrearOrdenItemUseCase addItem,
        ObtenerORdenUseCase getOrder)
        {
            _createOrder = createOrder;
            _addItem = addItem;
            _getOrder = getOrder;
        }
        // GET: OrdenController
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest req, CancellationToken ct)
        {
            try
            {
                var id = await _createOrder.ExecuteAsync(new(req.CustomerId, req.Currency), ct);
                return CreatedAtAction(nameof(GetById), new { orderId = id }, new { orderId = id });
            }
            catch (DomainException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        // GET: OrdenController/Details/5
        [HttpPost("{orderId:guid}/items")]
        public async Task<IActionResult> AddItem(Guid orderId, [FromBody] AddItemRequest req, CancellationToken ct)
        {
            try
            {
                var dto = await _addItem.ExecuteAsync(new(orderId, req.ProductId, req.Quantity, req.UnitPrice), ct);
                return Ok(dto);
            }
            catch (DomainException ex)
            {
                // Para este reto, lo mantenemos simple. Si quieres, podemos diferenciar 404 vs 400 por mensaje.
                var status = ex.Message.Contains("no encontrada", StringComparison.OrdinalIgnoreCase)
                    ? StatusCodes.Status404NotFound
                    : StatusCodes.Status400BadRequest;

                return Problem(detail: ex.Message, statusCode: status);
            }
        }

        // GET: OrdenController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdenController/Create


        // GET: OrdenController/Edit/5
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetById(Guid orderId, CancellationToken ct)
        {
            try
            {
                var dto = await _getOrder.ExecuteAsync(orderId, ct);
                return Ok(dto);
            }
            catch (DomainException ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        }

    }
    public sealed record CreateOrderRequest(string CustomerId, string Currency);
    public sealed record AddItemRequest(string ProductId, int Quantity, decimal UnitPrice);
}
