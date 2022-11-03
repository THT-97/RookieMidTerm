using Ecommerce.Data.Models;

namespace Ecommerce.API.ServiceInterfaces
{
    public interface ICategoryService : ICRUDService<Category>
    {
        public Task<int> CountProductsAsync(string categoryName);
        public Task<Category?> GetByNameAsync(string categoryName);
    }
}
