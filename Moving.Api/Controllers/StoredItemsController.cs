using Microsoft.AspNetCore.Mvc;
using Moving.Api.Contracts;
using Moving.Core.Models;
using Moving.Core.Services;

namespace Moving.Api.Controllers;

[ApiController]
[Route("api/storage-boxes/{boxId:guid}/items")]
[Produces("application/json")]
public class StoredItemsController : ControllerBase
{
    private readonly IStoredItemService _storedItemService;
    private readonly IStorageBoxService _storageBoxService;

    public StoredItemsController(
        IStoredItemService storedItemService,
        IStorageBoxService storageBoxService)
    {
        _storedItemService = storedItemService;
        _storageBoxService = storageBoxService;
    }

    /// <summary>Lista os itens de uma caixa.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StoredItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<StoredItem>>> GetByBox(Guid boxId)
    {
        if (await _storageBoxService.GetByIdAsync(boxId) is null)
            return NotFound();

        var items = await _storedItemService.GetByBoxIdAsync(boxId);
        return Ok(items);
    }

    /// <summary>Obtém um item pelo identificador dentro da caixa.</summary>
    [HttpGet("{itemId:guid}", Name = nameof(GetStoredItemById))]
    [ProducesResponseType(typeof(StoredItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StoredItem>> GetStoredItemById(Guid boxId, Guid itemId)
    {
        if (await _storageBoxService.GetByIdAsync(boxId) is null)
            return NotFound();

        var item = await _storedItemService.GetByIdAsync(itemId);
        if (item is null || item.StorageBoxId != boxId)
            return NotFound();

        return Ok(item);
    }

    /// <summary>Adiciona um item à caixa.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(StoredItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StoredItem>> Create(Guid boxId, [FromBody] CreateStoredItemRequest request)
    {
        if (await _storageBoxService.GetByIdAsync(boxId) is null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(request.ItemName))
            return BadRequest("ItemName é obrigatório.");

        var item = new StoredItem
        {
            ItemName = request.ItemName.Trim(),
            ItemDescription = request.ItemDescription,
            Keywords = request.Keywords,
            Quantity = request.Quantity
        };

        var created = await _storedItemService.CreateAsync(boxId, item);
        if (created is null)
            return NotFound();

        return CreatedAtAction(
            nameof(GetStoredItemById),
            new { boxId, itemId = created.Id },
            created);
    }

    /// <summary>Atualiza um item.</summary>
    [HttpPut("{itemId:guid}")]
    [ProducesResponseType(typeof(StoredItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StoredItem>> Update(
        Guid boxId,
        Guid itemId,
        [FromBody] UpdateStoredItemRequest request)
    {
        if (await _storageBoxService.GetByIdAsync(boxId) is null)
            return NotFound();

        var existing = await _storedItemService.GetByIdAsync(itemId);
        if (existing is null || existing.StorageBoxId != boxId)
            return NotFound();

        if (string.IsNullOrWhiteSpace(request.ItemName))
            return BadRequest("ItemName é obrigatório.");

        existing.ItemName = request.ItemName.Trim();
        existing.ItemDescription = request.ItemDescription;
        existing.Keywords = request.Keywords;
        existing.Quantity = request.Quantity;

        var updated = await _storedItemService.UpdateAsync(existing);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    /// <summary>Remove um item.</summary>
    [HttpDelete("{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid boxId, Guid itemId)
    {
        if (await _storageBoxService.GetByIdAsync(boxId) is null)
            return NotFound();

        var existing = await _storedItemService.GetByIdAsync(itemId);
        if (existing is null || existing.StorageBoxId != boxId)
            return NotFound();

        var deleted = await _storedItemService.DeleteAsync(itemId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
