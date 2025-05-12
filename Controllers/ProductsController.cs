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
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;
    private readonly IHubContext<InventoryHub> _hub;

    public ProductsController(IProductRepository repo, IHubContext<InventoryHub> hub)
    {
        _repo = repo;
        _hub = hub;
    }

  
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll()
    {
        var products = await _repo.GetAllAsync();

        var dtoList = products.Select(p => new ProductReadDto
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            Quantity = p.Quantity,
            Price = p.Price,
            CategoryName = p.Category?.Name,
            SupplierName = p.Supplier?.Name
        });
        
        await _hub.Clients.All.SendAsync("TestConnection", "Backend is live!");

        return Ok(dtoList);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReadDto>> GetById(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return NotFound();

        var dto = new ProductReadDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Quantity = product.Quantity,
            Price = product.Price,
            CategoryName = product.Category?.Name,
            SupplierName = product.Supplier?.Name
        };

        return Ok(dto);
    }

 
    [HttpPost]
    public async Task<ActionResult<Product>> Create(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId
        };

        var created = await _repo.AddAsync(product);
        
        await _hub.Clients.All.SendAsync("ProductAdded", new
        {
            created.ProductId, 
            created.Name,
            created.Description,
            created.Quantity,
            created.Price
        });
        
        return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, ProductDto dto)
    {
        if (id != dto.ProductId) return BadRequest();

        var product = await _repo.GetByIdAsync(id);
        if (product == null) return NotFound();

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Quantity = dto.Quantity;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;
        product.SupplierId = dto.SupplierId;

        await _repo.UpdateAsync(product);
        await _hub.Clients.All.SendAsync("ProductUpdated", new {
            product.ProductId,
            product.Name,
            product.Description,
            product.Quantity,
            product.Price
        });

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repo.DeleteAsync(id);
        if (!success) return NotFound();
        
        await _hub.Clients.All.SendAsync("ProductDeleted", id);
        
        return NoContent();
    }
}
