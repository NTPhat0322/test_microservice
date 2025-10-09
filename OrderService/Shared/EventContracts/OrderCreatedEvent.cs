namespace OrderService.Shared.EventContracts
{
    public record OrderCreatedEvent
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
