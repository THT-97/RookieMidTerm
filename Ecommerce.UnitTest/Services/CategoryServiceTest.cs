using Ecommerce.API.Data;
using Ecommerce.API.Services;
using Ecommerce.Data.Models;
using Ecommerce.DTO.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.UnitTest.Services
{
    public class CategoryServiceTest : IDisposable
    {
        private readonly DbContextOptions<EcommerceDbContext> _options;
        private readonly EcommerceDbContext _context;
        private readonly List<Category> _categories;
        private readonly List<Product> _products;
        public CategoryServiceTest()
        {
            //Implement in memory dbcontext for service testing
            _options = new DbContextOptionsBuilder<EcommerceDbContext>().UseInMemoryDatabase("CategoryTestDB").Options;
            _context = new EcommerceDbContext(_options);
            //Create fake data
            _categories = new()
            {
                new Category(){Id=1, Name="Cat 1", Description="Category 1", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Cat 2", Description="Category 2", Status=(byte)CommonStatus.NotAvailable},
                new Category(){Id=3, Name="Cat 3", Description="Category 3", Status=(byte)CommonStatus.Available},
                new Category(){Id=4, Name="Cat 4", Description="Category 4", Status=(byte)CommonStatus.Available},
            };

            _products = new()
            {
                new Product(){Name="Product 1", Category = _categories[0], Status=(byte)CommonStatus.Available},
                new Product(){Name="Product 2", Category = _categories[1], Status=(byte)CommonStatus.Available},
                new Product(){Name="Product 3", Category = _categories[1], Status=(byte)CommonStatus.NotAvailable},
                new Product(){Name="Product 4", Status=(byte)CommonStatus.Available},
                new Product(){Name="Product 5", Category = _categories[0], Status=(byte)CommonStatus.Available},
                new Product(){Name="Product 6", Category = _categories[3], Status=(byte)CommonStatus.Available},
            };
            _context.Database.EnsureDeleted();
            _context.Categories.AddRange(_categories);
            _context.Products.AddRange(_products);
            _context.SaveChanges();
        }

        [Fact]
        public void GetAllAsync()
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            List<Category>? result = categoryService.GetAllAsync()?.Result;
            Assert.NotNull(result);
            Assert.Equivalent(_categories.Where(c => c.Status != (byte)CommonStatus.NotAvailable)
                                         .ToList(), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetByIdAsync_NotNull (int id)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            Category? result = categoryService.GetByIDAsync(id).Result;
            Assert.NotNull(result);
            Assert.Equivalent(_categories.FirstOrDefault(c => c.Id == id
                                                              && c.Status != (byte)CommonStatus.NotAvailable),
                              result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(6)]
        public void GetByIdAsync_Null(int id)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            Category? result = categoryService.GetByIDAsync(id).Result;
            Assert.Null(result);
        }

        [Theory]
        [InlineData("Cat 1")]
        [InlineData("Cat 4")]
        [InlineData("Cat 3")]
        public void CountProductsAsync_Found(string categoryName)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            int result = categoryService.CountProductsAsync(categoryName).Result;
            Assert.Equal(_categories.FirstOrDefault(c => c.Name == categoryName && c.Status != (byte)CommonStatus.NotAvailable).Products.Count, result);
        }

        [Theory]
        [InlineData("Cat")]
        [InlineData("")]
        [InlineData("Something")]
        public void CountProductsAsync_NotFound(string categoryName)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            int result = categoryService.CountProductsAsync(categoryName).Result;
            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(1,2)]
        [InlineData(2, 2)]
        [InlineData(1, 6)]
        public void GetPageAsync_Success(int page, int limit)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);
            int realPage = page;
            int realLimit = limit;
            if (realPage > 0) realPage--;
            int count = _categories.Count;
            if (count < limit)
            {
                realPage = 0;
                realLimit = count;
            }

            //ACT
            List<Category>? result = categoryService.GetPageAsync(page, limit).Result;
            Assert.NotNull(result);
            Assert.Equivalent(_categories.Where(c => c.Status
                                                     != (byte)CommonStatus.NotAvailable)
                                         .Skip(realPage * realLimit)
                                         .Take(realLimit), result);
        }

        [Theory]
        [InlineData("Cat 1")]
        [InlineData("Cat 3")]
        [InlineData("Cat 4")]
        public async void GetByNameAsync_Success(string name)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);

            //ACT
            Category? result = await categoryService.GetByNameAsync(name);
            Assert.NotNull(result);
            Assert.Equivalent(_categories.FirstOrDefault(c => c.Name == name
                                                              && c.Status != (byte)CommonStatus.NotAvailable),
                              result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        public void DeleteAsync_Success(int id)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);
            List<Product>? products = _context.Products.Where(p => p.Category.Id == id)
                                                       .ToList();
            //ACT
            ActionResult result = categoryService.DeleteAsync(id).Result;
            Assert.IsType<OkResult>(result);
            Assert.Equal(_context.Categories.FirstOrDefault(c => c.Id == id)?.Status,
                         (byte)CommonStatus.NotAvailable);
            if(products != null)
            {
                foreach(Product product in products)
                    Assert.Equal(product?.Status, (byte)CommonStatus.NotAvailable);
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(61)]
        [InlineData(99)]
        public void DeleteAsync_NotFound(int id)
        {
            //ARRANGE
            CategoryService categoryService = new(_context);
            List<Product>? products = _context.Products.Where(p => p.Category.Id == id
                                                                   && p.Status != (byte)CommonStatus.NotAvailable)
                                                       .ToList();
            //ACT
            ActionResult result = categoryService.DeleteAsync(id).Result;
            Assert.IsType<NotFoundResult>(result);
        }

        //Clean up after test
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
