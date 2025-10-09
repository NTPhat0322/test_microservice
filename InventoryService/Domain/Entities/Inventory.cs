using System;
using System.Collections.Generic;

namespace InventoryService.Domain.Entities;

public partial class Inventory
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int QuantityInStock { get; set; }
}
