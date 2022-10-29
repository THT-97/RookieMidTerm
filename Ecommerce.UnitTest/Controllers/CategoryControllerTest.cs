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

            List<CategoryDTO> categoriesDTO = new()
            {
                new CategoryDTO(){Name="Cat 1", Description="Category 1", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Name="Cat 2", Description="Category 2", Status=(byte)CommonStatus.NotAvailable},
                new CategoryDTO(){Name="Cat 3", Description="Category 3", Status=(byte)CommonStatus.Available},
            };

            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetAllAsync()).ReturnsAsync(categories);
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            JsonResult? controllerResult = await categoryController.GetAll() as JsonResult;
            List<CategoryDTO> controllerDTO = (List<CategoryDTO>)controllerResult.Value;
            Assert.NotNull(controllerResult);
            Assert.Equivalent(categoriesDTO, controllerDTO);
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
            ActionResult controllerResult = await categoryController.GetAll();
            Assert.IsType<NotFoundResult>(controllerResult);
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
            ActionResult controllerResult = await categoryController.GetAll();
            Assert.IsType<BadRequestResult>(controllerResult);
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

            List<CategoryDTO> categoriesDTO = new()
            {
                new CategoryDTO(){Name="Shoes", Description="Shoes products", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Name="Boots", Description="Boots products", Status=(byte)CommonStatus.Available},
                new CategoryDTO(){Name="Sandals", Description="Sandal products", Status=(byte)CommonStatus.Available},
            };

            //Arranging Service
            _categoryServiceMoq.Setup(c => c.GetByIDAsync(id)).ReturnsAsync(categories.FirstOrDefault(c => c.Id == id));
            //Arranging Controller
            CategoryController categoryController = new CategoryController(_categoryServiceMoq.Object);

            //ACT
            //Controller test
            JsonResult? controllerResult = await categoryController.GetByID(id) as JsonResult;
            CategoryDTO controllerDTO = (CategoryDTO)controllerResult.Value;
            Assert.NotNull(controllerResult);
            Assert.Equivalent(categoriesDTO[id - 1], controllerDTO);
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
            var controllerResult = await categoryController.GetByID(id);
            Assert.IsType<NotFoundResult>(controllerResult);
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
            JsonResult? controllerResult = await categoryController.CountProducts(categoryName) as JsonResult;
            int count = (int) controllerResult.Value;
            Assert.NotNull(controllerResult);
            Assert.Equal(categories.FirstOrDefault(c => c.Name == categoryName).Products.Count, count);
        }
    }
}