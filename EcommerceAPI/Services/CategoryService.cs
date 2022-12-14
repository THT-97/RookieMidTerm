using Ecommerce.API.Data;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Services
{
    public class CategoryService : ICategoryService, IAsyncDisposable
    {
        private readonly EcommerceDbContext _context;

        public CategoryService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Categories.Where(c => c.Status != (byte)CommonStatus.NotAvailable)
                                            .CountAsync();
        }

        public async Task<List<Category>?> GetAllAsync()
        {
            return await _context.Categories.Where(c => c.Status != (byte)CommonStatus.NotAvailable)
                                            .ToListAsync();
        }

        public async Task<Category?> GetByIDAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id
                                                                      && c.Status != (byte)CommonStatus.NotAvailable);
        }

        public async Task<List<Category>?> GetPageAsync(int page, int limit)
        {
            int count = await _context.Categories.CountAsync();
            if (page > 0) page--;
            if (count < limit)
            {
                page = 0;
                limit = count;
            }
            List<Category>? categories = await _context.Categories.Where(c => c.Status != (byte)CommonStatus.NotAvailable)
                                                                  .Skip(page * limit)
                                                                  .Take(limit)
                                                                  .ToListAsync();
            return categories;
        }

        public async Task<int> CountProductsAsync(string categoryName)
        {
            Category? category = await _context.Categories.Include(c => c.Products.Where(p => p.Status != (byte)CommonStatus.NotAvailable))
                                                          .FirstOrDefaultAsync(c => c.Name == categoryName.Trim());
            if (category != null) return category.Products.Count;
            return -1;
        }

        public async Task<Category?> GetByNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower()
                                                                      && c.Status != (byte)CommonStatus.NotAvailable);
        }

        public async Task<ActionResult> CreateAsync(Category entry)
        {
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == entry.Name.ToLower()
                                                        && c.Status != (byte)CommonStatus.NotAvailable))
            {
                return new BadRequestResult();
            }
            entry.Status = (byte)CommonStatus.Available;
            _context.Categories.Add(entry);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult> UpdateAsync(int id, Category entry)
        {
            if (id != entry.Id) return new BadRequestResult();
            if (!_context.Categories.Any(c => c.Id == id)) return new NotFoundResult();
            try
            {
                _context.Categories.Attach(entry);
                _context.Entry(entry).State = EntityState.Modified;
                //Specify that Id is not modified so it won't be changed
                _context.Entry(entry).Property(c => c.Id).IsModified = false;
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch { return new BadRequestResult(); }
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            Category? target = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (target != null)
            {
                target.Status = (byte)CommonStatus.NotAvailable;
                List<Product>? products = await _context.Products.Where(p => p.Category.Id == target.Id).ToListAsync();
                if (products != null)
                {
                    foreach (Product product in products) product.Status = (byte)CommonStatus.NotAvailable;
                }
                
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            return new NotFoundResult();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }
}
