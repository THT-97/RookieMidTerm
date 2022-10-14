using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EcommerceAPI.Services
{
    public class ProductService : IGetService<Product>
    {
        private readonly EcommerceDbContext context;

        public ProductService(EcommerceDbContext dbContext)
        {
            context = dbContext;
        }

        public async Task<List<Product>>? GetAllProductsAsync()
        {
            return await context.Products.ToListAsync<Product>();
        }

        public async Task<Product>? GetProductAsync(int id)
        {
            return await context.Products
                                .Where(p=>p.Id==id)
                                .SingleOrDefaultAsync<Product>();
        }
        public async Task<List<Product>>? GetByCategoryAsync(int categoryId)
        {
            return await context.Products
                                .Where(p=>p.Category.Id == categoryId)
                                .ToListAsync<Product>();
        }
        //get new products (created within 90 days)
        public async Task<List<Product>>? GetNewAsync()
        {
            return await context.Products
                                .Where(p => p.CreatedDate.Year == DateTime.UtcNow.Year &&
                                            p.CreatedDate.Month >= DateTime.UtcNow.Month-3)
                                .ToListAsync<Product>();
        }
        //get products with high rating (rating > 3)
        public async Task<List<Product>>? GetHighRatingAsync()
        {
            return await context.Products
                                .Where(p => p.Rating > 3)
                                .ToListAsync<Product>();
        }
    }
}
