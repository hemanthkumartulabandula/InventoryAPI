using InventoryAPI.Models;

namespace InventoryAPI.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    
    Task<Product?> GetByIdAsync(int id);
    
    Task<Product> AddAsync(Product product);
    
    Task<Product> UpdateAsync(Product product);
    
    Task<bool> DeleteAsync(int id);
}