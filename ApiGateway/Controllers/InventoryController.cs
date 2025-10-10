using ApiGateway.DTOs;
using InventoryGrpc.Protos;
using Microsoft.AspNetCore.Http;
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
            if(rs.Success)
            {
                return Ok(rs);
            }
            return BadRequest("Cannot create inventory");
        }
    }
}
