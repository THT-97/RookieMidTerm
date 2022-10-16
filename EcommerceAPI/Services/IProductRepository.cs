using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Services
{
    public interface IProductRepository
    {
        public Task<List<Product>>? GetNewAsync();
        public Task<List<Product>>? GetByCategoryAsync(int categoryId);
        public Task<List<Product>>? GetHighRatingAsync();
    }
}
