using ApiGateway.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderGrpc.Protos;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderService.OrderServiceClient orderServiceClient) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await orderServiceClient.GetOrdersAsync(new EmptyRequest());
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] string id)
        {
            var order = await orderServiceClient.GetByIdAsync(new OrderIdRequest { Id = id });
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest createOrderRequest)
        {
            OrderInfo tmp = new()
            {
                ProductId = createOrderRequest.ProductId,
                Quantity = createOrderRequest.Quantity
            };
            var order = await orderServiceClient.CreateOrderAsync(tmp);
            return Ok(order);
        }
    }
}
