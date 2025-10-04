using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Protos;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductService.ProductServiceClient productServiceClient) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await productServiceClient.GetProductsAsync(new EmptyRequest());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] string id)
        {
            var product = await productServiceClient.GetByIdAsync(new ProductIdRequest { Id = id });
            return Ok(product);
        }
    }
}
