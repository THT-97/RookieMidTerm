using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class BrandService : ICRUDService<Brand>, IDisposable
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
