namespace API.Models.DTO.Categorys;

public class CategoryReadDto
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}