﻿using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class CategoryService : ICategoryService, IDisposable
    {
        private readonly EcommerceDbContext _context;

        public CategoryService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<List<Category>>? GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category>? GetByIDAsync(int id)
        {
            return await _context.Categories.Where(c => c.Id == id)
                                            .SingleOrDefaultAsync();
        }

        public async Task<ActionResult> CreateAsync(Category entry)
        {
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == entry.Name.ToLower()))
            {
                return new BadRequestResult();
            }
            _context.Categories.Add(entry);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult> UpdateAsync(int id, Category entry)
        {
            if (id != entry.Id) return new BadRequestResult();
            if (!_context.Categories.Any(c => c.Id == id)) return new NotFoundResult();
            _context.Categories.Update(entry);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            Category target = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(target != null)
            {
                _context.Categories.Remove(target);
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
