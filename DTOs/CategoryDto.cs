namespace InventoryAPI.DTOs;

public class CategoryDto
{
    public int CategoryId { get; set; }  // Used for update; ignored during creation
    
    public string Name { get; set; }
}