using EcommerceAPI.Data;
using EcommerceAPI.Models;
using System.Data.Common;

namespace EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private EcommerceDbContext _context;

        public ProductService(EcommerceDbContext dbContext)
        {
            _context = dbContext;
        }

        public List<Product>? GetAllProducts()
        {
            return _context.Products.ToList<Product>();
        }

        public Product? GetProduct(int id)
        {
            return null;
        }
    }
}
