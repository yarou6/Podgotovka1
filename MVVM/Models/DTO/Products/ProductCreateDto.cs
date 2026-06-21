namespace MVVM.Models.DTO.Products;

public class ProductCreateDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public uint Stock { get; set; }
    public uint CategoryId { get; set; }
    public string? CategoryName { get; set; }
}