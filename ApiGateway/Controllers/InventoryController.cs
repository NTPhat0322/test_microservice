using ApiGateway.DTOs;
using InventoryGrpc.Protos;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(InventoryService.InventoryServiceClient inventoryServiceClient) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateInventoryAsync([FromBody] CreateInventoryRequest request)
        {
            CreateRequest tmp = new()
            {
                ProductId = request.ProductId,
                QuantityInStock = request.QuantityInStock
            };
            var rs = await inventoryServiceClient.CreateInventoryAsync(tmp);
            if (rs.Success)
            {
                return Ok(rs);
            }
            return BadRequest("Cannot create inventory");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventoriesAsync()
        {
            var rs = await inventoryServiceClient.GetAllInventoryAsync(new EmptyRequest());
            List<InventoryDto> inventories = new();
            foreach (var item in rs.Items)
            {
                inventories.Add(new InventoryDto()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    QuantityInStock = item.QuantityInStock
                });
            }
            return Ok(inventories);
        }
    }
}
