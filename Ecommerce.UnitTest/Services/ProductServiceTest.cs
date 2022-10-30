using Ecommerce.API.Data;
using Ecommerce.Data.Models;
using Ecommerce.DTO.Enum;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.UnitTest.Services
{
    public class ProductServiceTest
    {
        private DbContextOptions<EcommerceDbContext> _options;
        private EcommerceDbContext _context;
        private List<Category> _categories;
        private List<Product> _products;

        public ProductServiceTest()
        {
            //Implement in memory dbcontext for service testing
            _options = new DbContextOptionsBuilder<EcommerceDbContext>().UseInMemoryDatabase("EcommerceTestDB").Options;
            _context = new EcommerceDbContext(_options);
            //Create fake data
            _categories = new()
            {
                new Category(){Name="Cat 1"},
                new Category(){Name="Cat 2"},
                new Category(){Name="Cat 3"},
            };

            _products = new()
            {
                new Product(){Id=1, Name="Product 1", Category = _categories[0]},
                new Product(){Id=2, Name="Product 2", Category = _categories[0]},
                new Product(){Id=3, Name="Product 3", Category = _categories[1]},
                new Product(){Id=4, Name="Product 4", Category = _categories[1]},
                new Product(){Id=5, Name="Product 5", Category = _categories[1]},
                new Product(){Id=6, Name="Product 6", Category = _categories[2]},
            };
        }
    }
}
