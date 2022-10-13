using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IGetService<T>
    {
        public List<T>? GetAllProducts();
        public T? GetProduct(int id);
    }
}
