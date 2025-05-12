namespace InventoryAPI.Repositories;
using InventoryAPI.Models;
using InventoryAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Data;

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryContext _context;

    public SupplierRepository(InventoryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers.ToListAsync();
    }

    
    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers.FindAsync(id);
    }
    
    public async Task<Supplier> AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

   
    public async Task<Supplier> UpdateAsync(Supplier supplier)
    {
        _context.Entry(supplier).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return supplier;
    }

  
    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
            return false;

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return true;
    }
}