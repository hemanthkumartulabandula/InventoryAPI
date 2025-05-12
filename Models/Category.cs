namespace InventoryAPI.Models;

public class Category
{
    public int CategoryId { get; set; }
    
    public string Name { get; set; }
    
    public ICollection<Product> Products { get; set; } // one to many, as we can have many products in one category
}