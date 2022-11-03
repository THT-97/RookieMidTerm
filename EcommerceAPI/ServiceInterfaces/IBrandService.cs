using Ecommerce.Data.Models;

namespace Ecommerce.API.ServiceInterfaces
{
    public interface IBrandService : ICRUDService<Brand>
    {
        public Task<int> CountProductsAsync(string brandName);
        public Task<Brand?> GetByNameAsync(string brandName);
    }
}
