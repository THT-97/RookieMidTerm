using Ecommerce.API.Repositories;
using Ecommerce.Data.Models;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Services
{
    public class RatingService : ICRUDService<Rating>, IAsyncDisposable
    {
        private readonly EcommerceDbContext _context;

        public RatingService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Ratings.CountAsync();
        }

        public async Task<ActionResult> CreateAsync(Rating entry)
        {
            Rating dup = await _context.Ratings.Where(r => r.Product.Id == entry.Product.Id
                                                           && r.User.Id == entry.User.Id)
                                               .FirstOrDefaultAsync();

            if (dup != null) return await UpdateAsync(dup.Id, entry);
            try
            {
                await _context.Ratings.AddAsync(entry);
                return new OkResult();
            }
            catch { return new BadRequestResult(); }
        }

        public Task<ActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Rating>>? GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Rating>? GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Rating>>? GetPage(int page, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> UpdateAsync(int id, Rating entry)
        {
            if (id != entry.Id) return new BadRequestResult();
            if (await _context.Ratings.AnyAsync(r => r.Id == id)) {
                _context.Entry(entry).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            return new NotFoundResult();
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return ((IAsyncDisposable)_context).DisposeAsync();
        }
    }
}
