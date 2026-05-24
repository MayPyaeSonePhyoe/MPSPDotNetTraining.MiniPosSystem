using Microsoft.EntityFrameworkCore;
using MPSPDotNetTraining.MiniPosSystem.Data;
using MPSPDotNetTraining.MiniPosSystem.Models.Entities;
using MPSPDotNetTraining.MiniPosSystem.Models.Requests;
using MPSPDotNetTraining.MiniPosSystem.Models.Responses;

namespace MPSPDotNetTraining.MiniPosSystem.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL PRODUCTS
        // =========================
        public async Task<List<ProductResponse>> GetAllAsync()
        {
            return await _context.Products
                .Select(x => new ProductResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    StockQty = x.StockQty,
                    CategoryId = x.CategoryId
                })
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                CategoryId = product.CategoryId
            };
        }

        // =========================
        // CREATE PRODUCT
        // =========================
        public async Task<ProductResponse?> CreateAsync(ProductRequest request)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId);

            if (!categoryExists)
                return null;

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                StockQty = request.StockQty,
                CategoryId = request.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                CategoryId = product.CategoryId
            };
        }

        // =========================
        // UPDATE PRODUCT
        // =========================
        public async Task<bool> UpdateAsync(int id, ProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return false;

            product.Name = request.Name;
            product.Price = request.Price;
            product.StockQty = request.StockQty;
            product.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }

        // =========================
        // DELETE PRODUCT
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // SEARCH PRODUCT
        // =========================
        public async Task<List<ProductResponse>> SearchAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword))
                .Select(x => new ProductResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    StockQty = x.StockQty,
                    CategoryId = x.CategoryId
                })
                .ToListAsync();
        }

        // =========================
        // STOCK INCREASE
        // =========================
        public async Task<bool> IncreaseStockAsync(int productId, int qty)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) return false;

            product.StockQty += qty;

            await _context.SaveChangesAsync();
            return true;
        }

        // =========================
        // STOCK DECREASE
        // =========================
        public async Task<bool> DecreaseStockAsync(int productId, int qty)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) return false;

            if (product.StockQty < qty)
                return false;

            product.StockQty -= qty;

            await _context.SaveChangesAsync();
            return true;
        }

        // =========================
        // CHECK STOCK
        // =========================
        public async Task<bool> HasStockAsync(int productId, int qty)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) return false;

            return product.StockQty >= qty;
        }
    }
}