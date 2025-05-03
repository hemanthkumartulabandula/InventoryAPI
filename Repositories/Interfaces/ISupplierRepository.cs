using InventoryAPI.Models;

namespace InventoryAPI.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllAsync();
    
    Task<Supplier?> GetByIdAsync(int id);
    
    Task<Supplier> AddAsync(Supplier supplier);
    
    Task<Supplier> UpdateAsync(Supplier supplier);
    
    Task<bool> DeleteAsync(int id);
}