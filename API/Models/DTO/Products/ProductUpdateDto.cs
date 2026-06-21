namespace API.Models.DTO.Products;

public class ProductUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public uint Stock { get; set; }
    public uint CategoryId { get; set; }
    public bool? IsActive { get; set; }
}