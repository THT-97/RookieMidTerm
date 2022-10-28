using Ecommerce.API.Data;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Ecommerce.DTO.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Services
{
    public class ProductService : ICRUDService<Product>, IProductService, IAsyncDisposable
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
                                          .Include(p => p.Ratings.OrderByDescending(r => r.Date))
                                          .ThenInclude(r => r.User)
                                          .SingleOrDefaultAsync();
        }

        public async Task<List<Product>>? GetByCategoryAsync(string categoryName, int page=0, int limit=6)
        {
            int count = await _context.Products.Where(p => p.Category.Name == categoryName).CountAsync();
            if (page > 0) page--;
            if (count < limit)
            {
                page = 0;
                limit = count;
            }
            return await _context.Products.Where(p => p.Category.Name == categoryName)
                                          .Skip(page * limit)
                                          .Take(limit)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .ToListAsync();
        }

        public async Task<List<Product>>? GetByBrandAsync(string brandName)
        {
            return await _context.Products.Where(p => p.Brand.Name == brandName)
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

        //get 30 products with highest rating (>3)
        public async Task<List<Product>>? GetHighRatingAsync()
        {
            return await _context.Products.Where(p => p.Rating > 3)
                                          .OrderByDescending(p => p.Rating)
                                          .Take(30)
                                          .Include(p => p.Category)
                                          .Include(p => p.Brand)
                                          .ToListAsync();
        }

        public async Task<ActionResult> CreateAsync(Product entry)
        {
            Product? dup = await _context.Products.FirstOrDefaultAsync(p => p.Name == entry.Name
                                                                            && p.Category.Name == entry.Category.Name
                                                                            && p.Brand.Name == entry.Brand.Name);
            if (dup != null) return await UpdateAsync(dup.Id, entry);
            try
            {
                entry.CreatedDate = DateTime.Now;
                entry.Status = (byte)CommonStatus.Available;
                await _context.Products.AddAsync(entry);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch { return new BadRequestResult(); }
        }

        public async Task<ActionResult> UpdateAsync(int id, Product entry)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult>? RateAsync(RatingDTO rating)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == rating.ProductId);
            IdentityUser? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == rating.UserEmail);
            Rating? rate = await _context.Ratings.FirstOrDefaultAsync(r => r.Product.Id == product.Id && r.User.Id == user.Id);

            if (product != null && user != null)
            {
                if (rate != null)
                {
                    //calculate average rating for existing user rating
                    product.Rating = (float)(product.Rating * product.RatingCount - rate.Points + rating.Rate) / product.RatingCount;
                    rate.Points = rating.Rate;
                    rate.Comment = rating.Comment;
                    rate.Date = rating.Date;
                }
                else
                {
                    if (product.RatingCount == null) product.RatingCount = 0;
                    if (product.Rating == null || product.Rating <= 0) product.Rating = rating.Rate;
                    else
                    {
                        //calculate average rating for new user rating
                        product.Rating = (float)(product.Rating * product.RatingCount + rating.Rate) / (product.RatingCount + 1);
                    }
                    product.RatingCount++;
                    rate = new Rating
                    {
                        User = user,
                        Points = rating.Rate,
                        Product = product,
                        Comment = rating.Comment,
                        Date = rating.Date,
                        Status = (byte)CommonStatus.Available
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

        public async Task<List<Product>>? GetPageAsync(int page=0, int limit=6)
        {
            int count = await CountAsync();
            if (page > 0) page--;
            if (count < limit)
            {
                page = 0;
                limit = count;
            }
            List<Product>? products = await _context.Products.Skip(page * limit)
                                                             .Take(limit)
                                                             .ToListAsync();
            return products;
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_context).DisposeAsync();
        }
    }
}
