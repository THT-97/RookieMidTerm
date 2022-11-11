using Ecommerce.API.Data;
using Ecommerce.API.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Services
{
    public class UserService : IUserService, IAsyncDisposable
    {
        private readonly EcommerceDbContext _context;

        public UserService(EcommerceDbContext context)
        {
            _context = context;
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> CreateAsync(IdentityUser entry)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IdentityUser>?> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public Task<IdentityUser?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityUser>?> GetPageAsync(int page, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateAsync(int id, IdentityUser entry)
        {
            throw new NotImplementedException();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
