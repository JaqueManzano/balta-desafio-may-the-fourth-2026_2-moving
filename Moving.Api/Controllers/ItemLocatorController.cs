using Microsoft.AspNetCore.Mvc;
using Moving.Api.Contracts;
using Moving.Core.Services;

namespace Moving.Api.Controllers;

[ApiController]
[Route("api/item-locator")]
[Produces("application/json")]
public class ItemLocatorController : ControllerBase
{
    private readonly IItemLocatorService _itemLocatorService;

    public ItemLocatorController(IItemLocatorService itemLocatorService)
    {
        _itemLocatorService = itemLocatorService;
    }

    /// <summary>Usa IA para localizar a caixa de um item a partir de uma frase do usuário.</summary>
    [HttpPost("locate")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> LocateByUserMessage(
        [FromBody] LocateItemRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Search))
            return BadRequest("O campo search é obrigatório.");

        var message = await _itemLocatorService.LocateItemAsync(request.Search, cancellationToken);
        return Ok(new { message });
    }
}
