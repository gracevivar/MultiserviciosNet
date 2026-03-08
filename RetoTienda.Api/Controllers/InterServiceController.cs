using Microsoft.AspNetCore.Mvc;

namespace RetoTienda.Api.Controllers;

[ApiController]
[Route("internal")]
public sealed class InterServiceController : ControllerBase
{
    private readonly IHttpClientFactory _http;

    public InterServiceController(IHttpClientFactory http) => _http = http;

    [HttpGet("ping-peer")]
    public async Task<IActionResult> PingPeer(CancellationToken ct)
    {
        var client = _http.CreateClient();
        var resp = await client.GetAsync("http://ordenes-api-peer:8080/health", ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        return Ok(new
        {
            ok = resp.IsSuccessStatusCode,
            peerResponse = body
        });
    }
}