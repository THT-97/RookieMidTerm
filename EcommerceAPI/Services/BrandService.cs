using Ecommerce.API.Data;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Services
{
    public class BrandService : IBrandService, IAsyncDisposable
    {
        private readonly EcommerceDbContext _context;

        public BrandService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Brands.CountAsync();
        }

        public async Task<List<Brand>>? GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand>? GetByIDAsync(int id)
        {
            return await _context.Brands.Where(b => b.Id == id)
                                            .SingleOrDefaultAsync();
        }

        public async Task<ActionResult> CreateAsync(Brand entry)
        {
            if (await _context.Brands.AnyAsync(b => b.Name.ToLower() == entry.Name.ToLower()))
            {
                return new BadRequestResult();
            }
            _context.Brands.Add(entry);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult> UpdateAsync(int id, Brand entry)
        {
            if (id != entry.Id) return new BadRequestResult();
            if (!_context.Brands.Any(b => b.Id == id)) return new NotFoundResult();
            _context.Brands.Update(entry);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            Brand target = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (target != null)
            {
                _context.Brands.Remove(target);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            return new NotFoundResult();
        }

        public Task<List<Brand>>? GetPageAsync(int page, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountProductsAsync(string brandName)
        {
            Brand? brand = await _context.Brands.Include(b => b.Products).FirstOrDefaultAsync(b => b.Name == brandName);
            if (brand != null) return brand.Products.Count;
            return -1;
        }

        public async Task<Brand?> GetByNameAsync(string brandName)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Name.Contains(brandName));
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
