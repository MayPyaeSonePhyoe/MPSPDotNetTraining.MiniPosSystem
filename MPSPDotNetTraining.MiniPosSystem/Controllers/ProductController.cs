using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPSPDotNetTraining.MiniPosSystem.Data;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;
using MPSPDotNetTraining.MiniPosSystem.Models.Requests;
using MPSPDotNetTraining.MiniPosSystem.Models.Responses;

namespace MPSPDotNetTraining.MiniPosSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products
                .AsNoTracking()
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQty = p.StockQty,
                    CategoryId = p.CategoryId
                })
                .ToList();

            return Ok(products);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQty = p.StockQty,
                    CategoryId = p.CategoryId
                })
                .FirstOrDefault();

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        public IActionResult Create([FromBody] ProductRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var categoryExists = _context.Categories
                .Any(c => c.Id == request.CategoryId);

            if (!categoryExists)
                return BadRequest("Invalid CategoryId");

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                StockQty = request.StockQty,
                CategoryId = request.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                CategoryId = product.CategoryId
            };

            return Ok(response);
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductRequest request)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            var categoryExists = _context.Categories.Any(c => c.Id == request.CategoryId);

            if (!categoryExists)
                return BadRequest("Invalid CategoryId");

            product.Name = request.Name;
            product.Price = request.Price;
            product.StockQty = request.StockQty;
            product.CategoryId = request.CategoryId;

            _context.SaveChanges();

            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                CategoryId = product.CategoryId
            };

            return Ok(response);
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("Product deleted successfully");
        }
    }
}