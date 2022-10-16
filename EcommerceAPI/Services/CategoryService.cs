﻿using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class CategoryService : ICRUDService<Category>, IDisposable
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
            if (await _context.Categories.AnyAsync(c => c.Name == entry.Name)) return new BadRequestResult();
            await _context.Categories.AddAsync(entry);
            return new OkResult();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
