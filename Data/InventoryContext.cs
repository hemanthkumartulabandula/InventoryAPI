using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InventoryAPI.Data;

public class InventoryContext : IdentityDbContext<User>
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options){}

    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    
}