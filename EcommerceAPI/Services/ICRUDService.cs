using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Services
{
    public interface ICRUDService<T>
    {
        public Task<List<T>>? GetAllAsync();
        public Task<T>? GetByIDAsync(int id);
        public Task<int> CountAsync();

        public Task<ActionResult> CreateAsync(T entry);
        public Task<ActionResult> UpdateAsync(int id, T entry);
        public Task<ActionResult> DeleteAsync(int id);
    }
}
