using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IGetService<T>
    {
        public Task<List<T>>? GetAllProductsAsync();
        public Task<T>? GetProductAsync(int id);
    }
}
