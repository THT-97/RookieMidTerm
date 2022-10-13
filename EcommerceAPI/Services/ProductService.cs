using EcommerceAPI.Data;
using EcommerceAPI.Models;
using System.Data.Common;

namespace EcommerceAPI.Services
{
    public class ProductService : IGetService<Product>
    {
        private readonly EcommerceDbContext context;

        public ProductService(EcommerceDbContext dbContext)
        {
            context = dbContext;
        }

        public List<Product>? GetAllProducts()
        {
            return context.Products.ToList<Product>();
        }

        public Product? GetProduct(int id)
        {
            return null;
        }
    }
}
