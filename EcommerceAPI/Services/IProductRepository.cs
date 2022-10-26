using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Services
{
    public interface IProductRepository
    {
        public Task<List<Product>>? GetNewAsync();
        public Task<List<Product>>? GetByCategoryAsync(string categoryName);
        public Task<List<Product>>? GetByBrandAsync(string brandName);
        public Task<List<Product>>? GetHighRatingAsync();
        public Task<IActionResult>? RateAsync(RatingDTO productRate);
    }
}
