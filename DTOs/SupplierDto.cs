namespace InventoryAPI.DTOs;

public class SupplierDto
{
    public int SupplierId { get; set; }  // Optional on create
    
    public string Name { get; set; }
    
    public string ContactEmail { get; set; }
    
    public string Phone { get; set; }
}