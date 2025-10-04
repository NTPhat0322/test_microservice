namespace ApiGateway.DTOs
{
    public class CreateOrderRequest
    {
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
