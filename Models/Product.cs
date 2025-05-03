namespace InventoryAPI.Models;

public class Product
{
    public int ProductId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    // Foreign key to Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // Foreign key to Supplier
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}