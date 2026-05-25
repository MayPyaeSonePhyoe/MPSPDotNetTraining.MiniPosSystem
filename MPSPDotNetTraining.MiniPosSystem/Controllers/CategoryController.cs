using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPSPDotNetTraining.MiniPosSystem.Data;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;
using MPSPDotNetTraining.MiniPosSystem.Models.Requests;

namespace MPSPDotNetTraining.MiniPosSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories
                .AsNoTracking()
                .ToList();

            return Ok(categories);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        // CREATE (ONLY NAME IN REQUEST)
        [HttpPost]
        public IActionResult Create([FromBody] CategoryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required");

            var category = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryRequest request)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("Category not found");

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required");

            category.Name = request.Name;

            _context.SaveChanges();

            return Ok(category);
        }

        // DELETE (WITH BUSINESS RULE)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("Category not found");

            bool hasProducts = _context.Products.Any(p => p.CategoryId == id);

            if (hasProducts)
            {
                return BadRequest(new
                {
                    message = "Cannot delete category because products still exist in this category."
                });
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Category deleted successfully"
            });
        }
    }
}