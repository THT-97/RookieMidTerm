using Ecommerce.Data.Models;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService : IProductRepository, IDisposable
    {
        private readonly EcommerceDbContext _context;


        public ProductService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>>? GetAllAsync()
        {
            List<Product> products = await _context.Products.Include(p => p.Brand)
                                                            .Include(p => p.Category)
                                                            .OrderByDescending(p => p.CreatedDate)
                                                            .ToListAsync();
            return products;
        }

        public async Task<Product>? GetByIDAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .SingleOrDefaultAsync();
        }
        
        public async Task<List<Product>>? GetByCategoryAsync(string categoryName)
        {
            return await _context.Products.Where(p => p.Category.Name == categoryName)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .ToListAsync();
        }

        //get 30 newest products
        public async Task<List<Product>>? GetNewAsync()
        {
            return await _context.Products.OrderByDescending(p => p.CreatedDate)
                                          .Take(30)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .ToListAsync();
        }
        
        //get 30 products with highest rating
        public async Task<List<Product>>? GetHighRatingAsync()
        {
            return await _context.Products.OrderByDescending(p => p.Rating)
                                          .Take(30)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .ToListAsync();
        }

        public Task<ActionResult> CreateAsync(Product entry)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateAsync(int id, Product entry)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
