using Ecommerce.Data.Models;

namespace EcommerceAPI.Services
{
    public interface IProductRepository
    {
        public Task<List<Product>>? GetNewAsync();
        public Task<List<Product>>? GetByCategoryAsync(string CategoryName);
        public Task<List<Product>>? GetHighRatingAsync();
    }
}
