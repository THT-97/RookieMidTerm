using Ecommerce.API.Data;
using Ecommerce.API.Services;
using Ecommerce.Data.Models;
using Ecommerce.DTO.Enum;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.UnitTest.Services
{
    public class CategoryServiceTest
    {
        private DbContextOptions<EcommerceDbContext> _options;
        private EcommerceDbContext _context;
        private List<Category> _categories;
        private List<Product> _products;
        public CategoryServiceTest()
        {
            //Implement in memory dbcontext for service testing
            _options = new DbContextOptionsBuilder<EcommerceDbContext>().UseInMemoryDatabase("EcommerceTestDB").Options;
            _context = new EcommerceDbContext(_options);
            //Create fake data
            _categories = new()
            {
                new Category(){Id=1, Name="Cat 1", Description="Category 1", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Cat 2", Description="Category 2", Status=(byte)CommonStatus.NotAvailable},
                new Category(){Id=3, Name="Cat 3", Description="Category 3", Status=(byte)CommonStatus.Available},
            };

            _products = new()
            {
                new Product(){Name="Product 1"},
                new Product(){Name="Product 2"},
                new Product(){Name="Product 3"},
                new Product(){Name="Product 4"},
                new Product(){Name="Product 5"},
                new Product(){Name="Product 6"},
            };
        }

        [Fact]
        public async Task GetAllAsync()
        {
            //ARRANGE
            CategoryService categoryService = new CategoryService(_context);
            _context.Database.EnsureDeleted();
            _context.Categories.AddRange(_categories);
            _context.SaveChanges();

            //ACT
            List<Category>? result = await categoryService.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equivalent(_categories, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsync_NotNull (int id)
        {
            //ARRANGE
            CategoryService categoryService = new CategoryService(_context);
            _context.Database.EnsureDeleted();
            _context.Categories.AddRange(_categories);
            _context.SaveChanges();

            //ACT
            Category? result = await categoryService.GetByIDAsync(id);
            Assert.NotNull(result);
            Assert.Equivalent(_categories.FirstOrDefault(c => c.Id == id), result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public async Task GetByIdAsync_Null(int id)
        {
            //ARRANGE
            CategoryService categoryService = new CategoryService(_context);

            //ACT
            Category? result = await categoryService.GetByIDAsync(id);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("Cat 1")]
        [InlineData("Cat 2")]
        [InlineData("Cat 3")]
        public async Task CountProductsAsync_Found(string categoryName)
        {
            //ARRANGE
            CategoryService categoryService = new CategoryService(_context);
            _categories[0].Products = new List<Product>() { _products[0] };
            _categories[1].Products = new List<Product>() { _products[1], _products[2], _products[3] };
            _categories[2].Products = new List<Product>() { _products[4], _products[5]};
            _context.Database.EnsureDeleted();
            _context.Categories.AddRange(_categories);
            _context.SaveChanges();

            //ACT
            int result = await categoryService.CountProductsAsync(categoryName);
            Assert.Equal(_categories.FirstOrDefault(c => c.Name == categoryName).Products.Count, result);
        }

        [Theory]
        [InlineData("Cat")]
        [InlineData("")]
        [InlineData("Something")]
        public async Task CountProductsAsync_NotFound(string categoryName)
        {
            //ARRANGE
            CategoryService categoryService = new CategoryService(_context);

            //ACT
            int result = await categoryService.CountProductsAsync(categoryName);
            Assert.Equal(-1, result);
        }
    }
}
