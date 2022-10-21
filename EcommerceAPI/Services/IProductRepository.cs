using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Services
{
    public interface IProductRepository
    {
        public Task<List<Product>>? GetNewAsync();
        public Task<List<Product>>? GetByCategoryAsync(string CategoryName);
        public Task<List<Product>>? GetHighRatingAsync();
        public Task<IActionResult>? RateAsync(ProductRateDTO productRate);
    }
}
