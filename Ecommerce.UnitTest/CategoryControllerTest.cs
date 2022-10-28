using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Ecommerce.DTO.Enum;
using EcommerceAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;

namespace Ecommerce.API.UnitTest
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _catServiceMoq;
        public CategoryControllerTest()
        {
            _catServiceMoq = new Mock<ICategoryService>();
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
            
            //Arranging Service test
            _catServiceMoq.Setup(c => c.GetAllAsync())
                         .ReturnsAsync(categories);
            //Arranging Controller test
            CategoryController catController = new CategoryController(_catServiceMoq.Object);

            //ACT
            List<Category> serviceResult = await _catServiceMoq.Object.GetAllAsync();
            Assert.NotNull(serviceResult);
            Assert.Equal(categories.ToJson(), serviceResult.ToJson());

            JsonResult? controllerResult = await catController.GetAll() as JsonResult;
            List<CategoryDTO> controllerDTO = (List<CategoryDTO>)controllerResult.Value;
            Assert.NotNull(controllerResult);
            Assert.Equal(controllerDTO.ToJson(), controllerDTO.ToJson());
        }
    }
}