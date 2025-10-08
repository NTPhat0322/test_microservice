namespace OrderService.Shared.EventContracts
{
    public record OrderCreatedEvent
    {
        public string Msg { get; set; } = null!;
    }
}
