namespace InventoryAPI.Models;

public class Supplier
{
    public int SupplierId { get; set; }
    
    public string Name { get; set; }
    
    public string ContactEmail { get; set; }
    
    public string Phone { get; set; }

    
    public ICollection<Product> Products { get; set; }
}