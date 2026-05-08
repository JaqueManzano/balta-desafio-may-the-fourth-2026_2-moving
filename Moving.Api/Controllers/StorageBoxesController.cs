using Microsoft.AspNetCore.Mvc;
using Moving.Api.Contracts;
using Moving.Core.Models;
using Moving.Core.Services;

namespace Moving.Api.Controllers;

[ApiController]
[Route("api/storage-boxes")]
[Produces("application/json")]
public class StorageBoxesController : ControllerBase
{
    private readonly IStorageBoxService _storageBoxService;

    public StorageBoxesController(IStorageBoxService storageBoxService)
    {
        _storageBoxService = storageBoxService;
    }

    /// <summary>Lista todas as caixas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StorageBox>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StorageBox>>> GetAll()
    {
        var boxes = await _storageBoxService.GetAllAsync();
        return Ok(boxes);
    }

    /// <summary>Obtém uma caixa pelo identificador.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StorageBox), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StorageBox>> GetById(Guid id)
    {
        var box = await _storageBoxService.GetByIdAsync(id);
        if (box is null)
            return NotFound();

        return Ok(box);
    }

    /// <summary>Cria uma nova caixa.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(StorageBox), StatusCodes.Status201Created)]
    public async Task<ActionResult<StorageBox>> Create([FromBody] CreateStorageBoxRequest request)
    {
        var box = new StorageBox
        {
            Description = request.Description
        };

        var created = await _storageBoxService.CreateAsync(box);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza uma caixa.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(StorageBox), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StorageBox>> Update(Guid id, [FromBody] UpdateStorageBoxRequest request)
    {
        var existing = await _storageBoxService.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        existing.Description = request.Description;

        await _storageBoxService.UpdateAsync(existing);
        return Ok(existing);
    }

    /// <summary>Remove uma caixa e seus itens.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await _storageBoxService.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        await _storageBoxService.DeleteAsync(id);
        return NoContent();
    }
}
