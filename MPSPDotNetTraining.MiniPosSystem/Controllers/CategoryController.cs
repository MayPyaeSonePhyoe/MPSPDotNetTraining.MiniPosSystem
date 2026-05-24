using Microsoft.AspNetCore.Mvc;
using MPSPDotNetTraining.MiniPosSystem.Data;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;

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

        // GET: api/category
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Categories.ToList());
        }

        // POST: api/category
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
                return BadRequest("Category name is required");

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}