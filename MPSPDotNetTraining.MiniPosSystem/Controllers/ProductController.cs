using Microsoft.AspNetCore.Mvc;
using MPSPDotNetTraining.MiniPosSystem.Models.Requests;
using MPSPDotNetTraining.MiniPosSystem.Services;

namespace MPSPDotNetTraining.MiniPosSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            if (products == null)
                return Ok(new List<object>());

            // FIX: Map items on the fly to inject a sequential 'sn' starting at 1
            var response = products.Select((p, index) => new
            {
                sn = index + 1,       // UI uses this for clean 1, 2, 3... numbering
                id = p.Id,            // Backend still exposes the real ID (2, 4...) for operations
                name = p.Name,
                price = p.Price,
                stockQty = p.StockQty,
                categoryId = p.CategoryId
            }).ToList();

            return Ok(response);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Product not found");

            return Ok(result);
        }

        // SEARCH
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var result = await _productService.SearchAsync(keyword);
            return Ok(result);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            var result = await _productService.CreateAsync(request);

            if (result == null)
                return BadRequest("Invalid CategoryId");

            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductRequest request)
        {
            var result = await _productService.UpdateAsync(id, request);

            if (!result)
                return NotFound("Product not found");

            return Ok("Updated successfully");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);

            if (!result)
                return NotFound("Product not found");

            return Ok("Deleted successfully");
        }

        // INCREASE STOCK
        [HttpPost("{id}/increase-stock")]
        public async Task<IActionResult> IncreaseStock(int id, [FromQuery] int qty)
        {
            var result = await _productService.IncreaseStockAsync(id, qty);

            if (!result)
                return NotFound("Product not found");

            return Ok("Stock increased");
        }

        // DECREASE STOCK
        [HttpPost("{id}/decrease-stock")]
        public async Task<IActionResult> DecreaseStock(int id, [FromQuery] int qty)
        {
            var result = await _productService.DecreaseStockAsync(id, qty);

            if (!result)
                return BadRequest("Invalid stock or product not found");

            return Ok("Stock decreased");
        }
    }
}