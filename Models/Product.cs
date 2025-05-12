namespace InventoryAPI.Models;

public class Product
{
    public int ProductId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    
    public int CategoryId { get; set; }
    public Category Category { get; set; }

   
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}