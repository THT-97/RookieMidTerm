using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Ecommerce.DTO.Enum;
using EcommerceAPI.Controllers;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;

namespace Ecommerce.API.UnitTest
{
    public class CategoryControllerTest
    {
        public CategoryControllerTest()
        {
            
        }

        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange
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
            
            var catMoq = new Mock<ICategoryService>();
            catMoq.Setup(c => c.GetAllAsync())
                  .Returns(Task.FromResult(categories));

            var catController = new CategoryController(catMoq.Object);

            //Act
            List<Category>? result = await catMoq.Object.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(categories, result);
        }
    }
}