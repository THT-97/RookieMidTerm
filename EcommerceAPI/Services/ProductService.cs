using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EcommerceAPI.Services
{
    public class ProductService : IGetService<Product>
    {
        private readonly EcommerceDbContext _context;

        public ProductService(EcommerceDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Product>>? GetAllAsync()
        {
            return await _context.Products.ToListAsync<Product>();
        }

        public async Task<Product>? GetByIDAsync(int id)
        {
            return await _context.Products
                                .Where(p=>p.Id==id)
                                .SingleOrDefaultAsync<Product>();
        }
        public async Task<List<Product>>? GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                                .Where(p=>p.Category.Id == categoryId)
                                .ToListAsync<Product>();
        }

        public async Task<int> CountAsync() => await _context.Products.CountAsync();

        //get 10 newest products (created within 3 months)
        public async Task<List<Product>>? GetNewAsync()
        {
            int count = await CountAsync();
            //newest records are at bottom of the data table
            int skip = count - 10; //skip to get newest records
            if (skip < 0) skip = 0;
            return await _context.Products
                                .Where(p => p.CreatedDate.Year == DateTime.UtcNow.Year &&
                                            p.CreatedDate.Month >= DateTime.UtcNow.Month-3)
                                .Skip(skip).OrderByDescending(p=>p.CreatedDate).ToListAsync<Product>();
        }
        //get products with high rating (rating > 3)
        public async Task<List<Product>>? GetHighRatingAsync()
        {
            return await _context.Products
                                .Where(p => p.Rating > 3)
                                .ToListAsync<Product>();
        }
    }
}
