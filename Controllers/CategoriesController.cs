using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Repositories.Interfaces;
using InventoryAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using InventoryAPI.Hubs;


namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repo;
    private readonly IHubContext<InventoryHub> _hub;

    public CategoriesController(ICategoryRepository repo, IHubContext<InventoryHub> hub)
    {
        _repo = repo;
        _hub = hub;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _repo.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create(CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        var created = await _repo.AddAsync(category);
        
        await _hub.Clients.All.SendAsync("CategoryAdded", new {
            created.CategoryId,
            created.Name
        });

        
        return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, CategoryDto dto)
    {
        if (id != dto.CategoryId) return BadRequest();

        var category = await _repo.GetByIdAsync(id);
        if (category == null) return NotFound();

        category.Name = dto.Name;

        await _repo.UpdateAsync(category);
        
        await _hub.Clients.All.SendAsync("CategoryUpdated", new {
            category.CategoryId,
            category.Name
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repo.DeleteAsync(id);
        if (!success) return NotFound();
        
        await _hub.Clients.All.SendAsync("CategoryDeleted", id);
        
        return NoContent();
    }
}