using InventoryAPI.Models;

namespace InventoryAPI.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    
    Task<Category?> GetByIdAsync(int id);
    
    Task<Category> AddAsync(Category category);
    
    Task<Category> UpdateAsync(Category category);
    
    Task<bool> DeleteAsync(int id);
}