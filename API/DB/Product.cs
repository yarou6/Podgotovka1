using System;
using System.Collections.Generic;

namespace API.DB;

public partial class Product
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public uint Stock { get; set; }

    public bool? IsActive { get; set; }

    public uint CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;
}
