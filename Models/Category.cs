namespace InventoryAPI.Models;

public class Category
{
    public int CategoryId { get; set; }
    
    public string Name { get; set; }
    
    // One-to-many: One category → many products
    public ICollection<Product> Products { get; set; }
}