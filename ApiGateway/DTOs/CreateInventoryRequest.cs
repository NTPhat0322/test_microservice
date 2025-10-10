namespace ApiGateway.DTOs
{
    public class CreateInventoryRequest
    {
        public string ProductId { get; set; } = null!;
        public int QuantityInStock { get; set; }
    }
}
