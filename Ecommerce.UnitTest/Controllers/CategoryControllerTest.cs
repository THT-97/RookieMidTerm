using Ecommerce.API.Controllers;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Ecommerce.DTO.Enum;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;

namespace Ecommerce.API.UnitTest.Controllers
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _categoryServiceMoq;
        public CategoryControllerTest()
        {
            _categoryServiceMoq = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task GetAll_Success()
        {
            //ARRANGE
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Cat 1", Description="Category 1", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Cat 2", Description="Category 2", Status=(byte)CommonStatus.NotAvailable},
                new Category(){Id=3, Name="Cat 3", Description="Category 3", Status=(byte)CommonStatus.Available},
            };

            List<CategoryDTO> expected = new()
            {
                new CategoryDTO(){Id=1, Name="Cat 1", Description="Category 1", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Id=2, Name="Cat 2", Description="Category 2", Status=(byte)CommonStatus.NotAvailable},
                new CategoryDTO(){Id=3, Name="Cat 3", Description="Category 3", Status=(byte)CommonStatus.Available},
            };

            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            JsonResult? result = await categoryController.GetAll() as JsonResult;
            List<CategoryDTO> categoriesDTO = (List<CategoryDTO>)result.Value;
            Assert.NotNull(result);
            Assert.Equivalent(expected, categoriesDTO);
        }

        [Fact]
        public async Task GetAll_NotFound()
        {
            //ARRANGE
            List<Category> categories = new();
            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            ActionResult result = await categoryController.GetAll();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_BadRequest()
        {
            //ARRANGE
            List<Category>? categories = null;
            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT

            //Controller test
            ActionResult result = await categoryController.GetAll();
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByID_Success(int id)
        {
            //ARRANGE
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new Category(){Id=3, Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };

            List<CategoryDTO> expected = new()
            {
                new CategoryDTO(){Id=1, Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Id=2, Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Id=3, Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };

            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetByIDAsync(id)).ReturnsAsync(categories.FirstOrDefault(c => c.Id == id));
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            //Controller test
            JsonResult? result = await categoryController.GetByID(id) as JsonResult;
            CategoryDTO categoriesDTO = (CategoryDTO)result.Value;
            Assert.NotNull(result);
            Assert.Equivalent(expected[id - 1], categoriesDTO);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public async Task GetByID_NotFound(int id)
        {
            //ARRANGE
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new Category(){Id=3, Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };

            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetByIDAsync(id)).ReturnsAsync(categories.FirstOrDefault(c => c.Id == id));
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            //Controller test
            var result = await categoryController.GetByID(id);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("Shoes")]
        [InlineData("Boots")]
        [InlineData("Sandals")]
        public async Task CountProducts_Success(string categoryName)
        {
            //ARRANGE
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new Category(){Id=3, Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };
            categories[0].Products = new List<Product>() { new Product()};
            categories[1].Products = new List<Product>() { new Product(), new Product(), new Product(), new Product() };
            categories[2].Products = new List<Product>() { new Product(), new Product() };
            //Arranging Service
            _categoryServiceMoq.Setup(c => c.CountProductsAsync(categoryName))
                         .ReturnsAsync(categories.FirstOrDefault(c => c.Name == categoryName).Products.Count);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);
            
            //ACT
            //Controller test
            JsonResult? result = await categoryController.CountProducts(categoryName) as JsonResult;
            int count = (int) result.Value;
            Assert.NotNull(result);
            Assert.Equal(categories.FirstOrDefault(c => c.Name == categoryName).Products.Count, count);
        }

        [Theory]
        [InlineData("Sneakers")]
        [InlineData("sad")]
        [InlineData("bruh")]
        public async Task CountProducts_Notfound(string categoryName)
        {
            //ARRANGE
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new Category(){Id=2, Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new Category(){Id=3, Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };
            categories[0].Products = new List<Product>() { new Product() };
            categories[1].Products = new List<Product>() { new Product(), new Product(), new Product(), new Product() };
            categories[2].Products = new List<Product>() { new Product(), new Product() };
            //Arranging Service
            _categoryServiceMoq.Setup(c => c.CountProductsAsync(categoryName))
                               .ReturnsAsync(-1);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            //Controller test
            var result = await categoryController.CountProducts(categoryName);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1,2)]
        [InlineData(2,2)]
        [InlineData(0,6)]
        [InlineData(1,6)]
        [InlineData(1,7)]
        public async Task GetPage_Success(int page, int limit)
        {
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Cat 1",},
                new Category(){Id=2, Name="Cat 2",},
                new Category(){Id=3, Name="Cat 3",},
                new Category(){Id=4, Name="Cat 4",},
                new Category(){Id=5, Name="Cat 5",},
                new Category(){Id=6, Name="Cat 6",},
            };

            List<CategoryDTO> expected = new()
            {
                new CategoryDTO(){Id=1, Name="Cat 1",},
                new CategoryDTO(){Id=2, Name="Cat 2",},
                new CategoryDTO(){Id=3, Name="Cat 3",},
                new CategoryDTO(){Id=4, Name="Cat 4",},
                new CategoryDTO(){Id=5, Name="Cat 5",},
                new CategoryDTO(){Id=6, Name="Cat 6",},
            };

            //Arranging Service
            int realPage = page;
            int realLimit = limit;
            if (realPage > 0) realPage--;
            int count = categories.Count;
            if (count < limit)
            {
                realPage = 0;
                realLimit = count;
            }
            _categoryServiceMoq.Setup(c => c.GetPageAsync(page, limit))
                  .ReturnsAsync(categories.Skip(realPage * realLimit)
                                          .Take(realLimit)
                                          .ToList());
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);
            //ACT
            JsonResult? result = await categoryController.GetPage(page, limit) as JsonResult;
            List<CategoryDTO>? categoryDTOs = (List<CategoryDTO>?)result.Value;
            Assert.NotNull(result);
            Assert.NotNull(categoryDTOs);
            Assert.NotEmpty(categoryDTOs);
            Assert.Equivalent(expected.Skip(realPage * realLimit)
                                      .Take(realLimit), categoryDTOs);
        }

        [Theory]
        [InlineData(2, 6)]
        [InlineData(4, 2)]
        [InlineData(5, 2)]
        public async Task GetPage_NotFound(int page, int limit)
        {
            List<Category> categories = new()
            {
                new Category(){Id=1, Name="Cat 1",},
                new Category(){Id=2, Name="Cat 2",},
                new Category(){Id=3, Name="Cat 3",},
                new Category(){Id=4, Name="Cat 4",},
                new Category(){Id=5, Name="Cat 5",},
                new Category(){Id=6, Name="Cat 6",},
            };

            //Arranging Service
            int realPage = page;
            int realLimit = limit;
            if (realPage > 0) realPage--;
            int count = categories.Count;
            if (count < limit)
            {
                realPage = 0;
                realLimit = count;
            }
            _categoryServiceMoq.Setup(c => c.GetPageAsync(page, limit))
                  .ReturnsAsync(categories.Skip(realPage * realLimit)
                                          .Take(realLimit)
                                          .ToList());
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);
            //ACT
            ActionResult? result = await categoryController.GetPage(page, limit);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
    }
    
}