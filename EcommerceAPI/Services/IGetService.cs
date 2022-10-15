using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public interface IGetService<T>
    {
        public Task<List<T>>? GetAllAsync();
        public Task<T>? GetByIDAsync(int id);
        public Task<int> CountAsync();
    }
}
