using Microsoft.AspNetCore.Mvc;
using MPSPDotNetTraining.MiniPosSystem.DTOs;
using MPSPDotNetTraining.MiniPosSystem.Services;

namespace MPSPDotNetTraining.MiniPosSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _saleService;

        public SaleController(SaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public IActionResult CreateSale([FromBody] CreateSaleRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Sale must contain at least one item.");
            }

            try
            {
                var result = _saleService.CreateSale(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.SaleId },   // ✅ FIXED HERE
                    result
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var sales = _saleService.GetAllSales();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred retrieving sales.",
                    details = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var sale = _saleService.GetSaleById(id);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}