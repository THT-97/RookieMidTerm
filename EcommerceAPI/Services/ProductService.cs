using AutoMapper;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService : ICRUDService<Product>, IProductRepository, IDisposable
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
                                                            .ToListAsync();
            return products;
        }

        public async Task<Product>? GetByIDAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id)
                                          .SingleOrDefaultAsync();
        }
        
        public async Task<List<Product>>? GetByCategoryAsync(string categoryName)
        {
            return await _context.Products.Where(p => p.Category.Name == categoryName)
                                          .ToListAsync();
        }

        //get 10 newest products (created within 3 months)
        public async Task<List<Product>>? GetNewAsync()
        {
            int count = await CountAsync();
            //newest records are at bottom of the data table
            int skip = count - 10; //calculate skipped records to get 10 newest records
            if (skip < 0) skip = 0;
            return await _context.Products.Where(p => p.CreatedDate.Year == DateTime.Now.Year
                                                      && p.CreatedDate.Month >= DateTime.Now.Month - 3)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .Skip(skip)
                                          .OrderByDescending(p => p.CreatedDate)
                                          .ToListAsync();
        }
        
        //get products with high rating (rating > 3)
        public async Task<List<Product>>? GetHighRatingAsync()
        {
            return await _context.Products
                                .Where(p => p.Rating > 3)
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
