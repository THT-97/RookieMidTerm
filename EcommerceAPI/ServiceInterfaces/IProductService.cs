using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.ServiceInterfaces
{
    public interface IProductService : ICRUDService<Product>
    {
        public Task<List<Product>?> GetNewAsync();
        public Task<List<Product>?> GetByCategoryAsync(string categoryName, int page, int limit);
        public Task<List<Product>?> GetByBrandAsync(string brandName, int page, int limit);
        public Task<List<Product>?> GetHighRatingAsync();
        public Task<IActionResult> RateAsync(RatingDTO productRate);
    }
}
