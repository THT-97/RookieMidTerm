using Ecommerce.API.Services;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService : ICRUDService<Product>, IProductRepository, IDisposable
    {
        private readonly EcommerceDbContext _context;
        private RatingService _ratingService;

        public ProductService(EcommerceDbContext context)
        {
            _context = context;
            _ratingService = new RatingService(_context);
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
                                          .Include(p => p.Ratings)
                                          .ThenInclude(r => r.User)
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

        public async Task<IActionResult>? RateAsync(ProductRateDTO productRate)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productRate.ProductId);
            IdentityUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == productRate.UserEmail);
            Rating? rate = await _context.Ratings.FirstOrDefaultAsync(r => r.Product.Id == product.Id && r.User.Id == user.Id);

            if (product != null && user != null)
            {
                if (rate != null)
                {
                    //calculate average rating for existing user rating
                    product.Rating = (float)(product.Rating * product.RatingCount - rate.Points + productRate.Rate) / product.RatingCount;
                    rate.Points = productRate.Rate;
                    rate.Comment = productRate.Comment;
                    rate.Date = productRate.Date;
                }
                else
                {
                    if (product.RatingCount == null) product.RatingCount = 0;
                    if (product.Rating == null || product.Rating <= 0) product.Rating = productRate.Rate;
                    else
                    {
                        //calculate average rating for new user rating
                        product.Rating = (float)(product.Rating * product.RatingCount + productRate.Rate) / (product.RatingCount + 1);
                        product.RatingCount++;
                    }
                    _context.Entry(product).State = EntityState.Modified;
                    rate = new Rating
                    {
                        User = user,
                        Points = productRate.Rate,
                        Product = product,
                        Comment = productRate.Comment,
                        Date = productRate.Date
                    };
                }
                try
                {
                    await _ratingService.CreateAsync(rate);
                    await _context.SaveChangesAsync();
                    return new OkResult();
                }
                catch { return new BadRequestResult(); }
            }
            return new BadRequestResult();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
