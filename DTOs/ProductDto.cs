namespace InventoryAPI.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }   
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }

    public int CategoryId { get; set; }  // F key
    public int SupplierId { get; set; }  // F key
}