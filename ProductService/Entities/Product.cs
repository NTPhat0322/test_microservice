using System;
using System.Collections.Generic;

namespace ProductService.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
}
