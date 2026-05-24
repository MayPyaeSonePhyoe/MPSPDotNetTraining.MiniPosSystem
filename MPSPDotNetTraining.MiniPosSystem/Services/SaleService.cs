using Microsoft.EntityFrameworkCore;
using MPSPDotNetTraining.MiniPosSystem.Data;
using MPSPDotNetTraining.MiniPosSystem.DTOs;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;
using MPSPDotNetTraining.MiniPosSystem.Models.Responses;

namespace MPSPDotNetTraining.MiniPosSystem.Services
{
    public class SaleService
    {
        private readonly AppDbContext _context;

        public SaleService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // CREATE SALE (DTO RESPONSE)
        // =========================
        public SaleResponse CreateSale(CreateSaleRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                throw new Exception("Sale must have at least one item");

            var productIds = request.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionary(p => p.Id);

            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                SaleItems = new List<SaleItem>(),
                TotalAmount = 0
            };

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new Exception("Quantity must be greater than 0");

                if (!products.TryGetValue(item.ProductId, out var product))
                    throw new Exception($"Product with ID {item.ProductId} not found");

                if (product.StockQty < item.Quantity)
                    throw new Exception($"Not enough stock for product: {product.Name}");

                var subTotal = product.Price * item.Quantity;

                product.StockQty -= item.Quantity;

                sale.SaleItems.Add(new SaleItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    SubTotal = subTotal
                });

                sale.TotalAmount += subTotal;
            }

            _context.Sales.Add(sale);
            _context.SaveChanges();

            return new SaleResponse
            {
                SaleId = sale.Id,
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount,
                Items = sale.SaleItems.Select(x => new SaleItemResponse
                {
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    SubTotal = x.SubTotal
                }).ToList()
            };
        }

        // =========================
        // GET ALL SALES (DTO FIXED)
        // =========================
        public List<SaleResponse> GetAllSales()
        {
            return _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .AsNoTracking()
                .Select(s => new SaleResponse
                {
                    SaleId = s.Id,
                    SaleDate = s.SaleDate,
                    TotalAmount = s.TotalAmount,

                    Items = s.SaleItems.Select(x => new SaleItemResponse
                    {
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        SubTotal = x.SubTotal
                    }).ToList()
                })
                .ToList();
        }

        // =========================
        // GET BY ID (DTO FIXED)
        // =========================
        public SaleResponse GetSaleById(int id)
        {
            var sale = _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);

            if (sale == null)
                throw new Exception("Sale not found");

            return new SaleResponse
            {
                SaleId = sale.Id,
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount,

                Items = sale.SaleItems.Select(x => new SaleItemResponse
                {
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    SubTotal = x.SubTotal
                }).ToList()
            };
        }
    }
}