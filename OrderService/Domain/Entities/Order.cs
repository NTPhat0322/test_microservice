using System;
using System.Collections.Generic;

namespace OrderService.Domain.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}
