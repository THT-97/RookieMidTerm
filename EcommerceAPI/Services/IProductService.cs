using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IProductService
    {
        public List<Product>? GetAllProducts();
        public Product? GetProduct(int id);
    }
}
