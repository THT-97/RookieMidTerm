using Ecommerce.API.Controllers;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ecommerce.API.UnitTest.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _productServiceMoq;
        public ProductControllerTest()
        {
            _productServiceMoq = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetNew_Success()
        {
            //ARRANGE
            //list of more than 30 products
            List<Product> products = new()
            {
                new Product{CreatedDate = DateTime.Now.AddDays(-1)}, new Product{CreatedDate = DateTime.Now.AddDays(-1)}, new Product{CreatedDate = DateTime.Now.AddDays(-2)},
                new Product{CreatedDate = DateTime.Now.AddDays(-2)}, new Product{CreatedDate = DateTime.Now}, new Product{CreatedDate = DateTime.Now.AddDays(-3)},
                new Product{CreatedDate = DateTime.Now.AddDays(-3)}, new Product{CreatedDate = DateTime.Now.AddDays(-3)}, new Product{CreatedDate = DateTime.Now},
                new Product{CreatedDate = DateTime.Now.AddDays(-5)}, new Product{CreatedDate = DateTime.Now.AddDays(-4)}, new Product{CreatedDate = DateTime.Now.AddMinutes(-16)},
                new Product{CreatedDate = DateTime.Now.AddDays(-7)}, new Product{CreatedDate = DateTime.Now.AddDays(-11)}, new Product{CreatedDate = DateTime.Now.AddDays(-21)},
                new Product{CreatedDate = DateTime.Now.AddDays(-9)}, new Product{CreatedDate = DateTime.Now.AddDays(-22)}, new Product{CreatedDate = DateTime.Now.AddDays(-32)},
                new Product{CreatedDate = DateTime.Now.AddDays(-13)}, new Product{CreatedDate = DateTime.Now.AddDays(-13)}, new Product{CreatedDate = DateTime.Now},
                new Product{CreatedDate = DateTime.Now.AddDays(-10)}, new Product{CreatedDate = DateTime.Now.AddDays(-14)}, new Product{CreatedDate = DateTime.Now.AddMinutes(-31)},
                new Product{CreatedDate = DateTime.Now.AddDays(-15)}, new Product{CreatedDate = DateTime.Now.AddDays(-42)}, new Product{CreatedDate = DateTime.Now.AddMinutes(-52)},
                new Product{CreatedDate = DateTime.Now.AddDays(-16)}, new Product{CreatedDate = DateTime.Now.AddDays(-43)}, new Product{CreatedDate = DateTime.Now.AddMinutes(-44)},
                new Product{CreatedDate = DateTime.Now.AddDays(-17)}, new Product{CreatedDate = DateTime.Now.AddDays(-44)}, new Product{CreatedDate = DateTime.Now.AddMinutes(-51)}
            };
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetNewAsync())
                              .ReturnsAsync(products.OrderByDescending(p => p.CreatedDate)
                                                    .Take(30)
                                                    .ToList());
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.GetNew() as JsonResult;
            List<ProductDTO>? productDTO = (List<ProductDTO>?)result.Value;
            Assert.NotNull(result);
            Assert.NotNull(productDTO);
            Assert.NotEmpty(productDTO);
            Assert.Equal(30, productDTO.Count);
        }

        [Fact]
        public async Task GetNew_NotFound()
        {
            //ARRANGE
            //list of more than 30 products
            List<Product> products = new();
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetNewAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object);

            //ACT
            ActionResult result = await productController.GetNew();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetNew_BadRequest()
        {
            //ARRANGE
            //list of more than 30 products
            List<Product> products = null;
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetNewAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object);

            //ACT
            ActionResult result = await productController.GetNew();
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
