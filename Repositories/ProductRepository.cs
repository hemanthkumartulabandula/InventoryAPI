
namespace InventoryAPI.Repositories;
using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Repositories.Interfaces;

public class ProductRepository : IProductRepository
{
    private readonly InventoryContext _context;

    public ProductRepository(InventoryContext context)
    {
        _context = context;
    }

 
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

        return products;
    }

   
    public async Task<Product?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        return product;
    }

    
    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

  
    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

 
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}