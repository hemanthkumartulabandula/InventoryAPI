using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Models;
using InventoryAPI.Repositories.Interfaces;
using InventoryAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierRepository _repo;

    public SuppliersController(ISupplierRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> GetAll()
    {
        var suppliers = await _repo.GetAllAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetById(int id)
    {
        var supplier = await _repo.GetByIdAsync(id);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<Supplier>> Create(SupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            ContactEmail = dto.ContactEmail,
            Phone = dto.Phone
        };

        var created = await _repo.AddAsync(supplier);
        return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SupplierDto dto)
    {
        if (id != dto.SupplierId) return BadRequest();

        var supplier = await _repo.GetByIdAsync(id);
        if (supplier == null) return NotFound();

        supplier.Name = dto.Name;
        supplier.ContactEmail = dto.ContactEmail;
        supplier.Phone = dto.Phone;

        await _repo.UpdateAsync(supplier);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repo.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}