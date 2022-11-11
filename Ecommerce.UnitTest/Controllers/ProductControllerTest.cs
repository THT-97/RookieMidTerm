using Ecommerce.API.Controllers;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NuGet.Protocol;
using System.Collections.Generic;

namespace Ecommerce.API.UnitTest.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _productServiceMoq;
        private readonly Mock<IBrandService> _brandServiceMoq;
        private readonly Mock<ICategoryService> _categoryServiceMoq;
        public ProductControllerTest()
        {
            _productServiceMoq = new Mock<IProductService>();
            _brandServiceMoq = new Mock<IBrandService>();
            _categoryServiceMoq = new Mock<ICategoryService>();
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
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.GetNew() as JsonResult;
            List<ProductDTO>? productDTO = (List<ProductDTO>?)result?.Value;
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
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult result = await productController.GetNew();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetNew_BadRequest()
        {
            //ARRANGE
            //list of more than 30 products
            List<Product>? products = null;
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetNewAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult result = await productController.GetNew();
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetHighRatings_Success()
        {
            //ARRANGE
            //List of more than 30 products
            List<Product> products = new()
            {
                new Product{Rating = 3.5f}, new Product{Rating = 4}, new Product{Rating = 2}, new Product{Rating = 1}, new Product{Rating = 5},
                new Product{Rating = 1.5f}, new Product{Rating = 1}, new Product{Rating = 4}, new Product{Rating = 5}, new Product{Rating = 4.5f},
                new Product{Rating = 2.4f}, new Product{Rating = 4.5f}, new Product{Rating = 4.2f}, new Product{Rating = 3.1f}, new Product{Rating = 4},
                new Product{Rating = 3.3f}, new Product{Rating = 3.25f}, new Product{Rating = 426f}, new Product{Rating = 4.71f}, new Product{Rating = 375f},
                new Product{Rating = 4.25f}, new Product{Rating = 4}, new Product{Rating = 4.23f}, new Product{Rating = 4.22f}, new Product{Rating = 3.5f},
                new Product{Rating = 5}, new Product{Rating = 3.1f}, new Product{Rating = 5}, new Product{Rating = 3.45f}, new Product{Rating = 3.49f},
                new Product{Rating = 4.9f}, new Product{Rating = 4}, new Product{Rating = 4}, new Product{Rating = 3.29f}, new Product{Rating = 4.5f}
        };
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetHighRatingAsync())
                              .ReturnsAsync(products.Where(p => p.Rating > 3)
                                                    .OrderByDescending(p => p.CreatedDate)
                                                    .Take(30)
                                                    .ToList());
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.GetHighRatings() as JsonResult;
            List<ProductDTO>? productDTO = (List<ProductDTO>?)result?.Value;
            Assert.NotNull(result);
            Assert.NotNull(productDTO);
            Assert.NotEmpty(productDTO);
            Assert.Equal(30, productDTO.Count);
            foreach (ProductDTO dto in productDTO) Assert.True(dto.Rating > 3);
        }

        [Fact]
        public async Task GetHighRatings_NotFound()
        {
            //ARRANGE
            //list of low rating products (rating <= 3)
            List<Product> products = new()
            {
                new Product{Rating = 3}, new Product{Rating = 2.4f}, new Product{Rating = 2},
                new Product{Rating = 1.5f}, new Product{Rating = 2.5f}, new Product{Rating = 1.5f},
                new Product{Rating = 2}, new Product{Rating = 1.4f}, new Product{Rating = 2.95f},
            };
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetHighRatingAsync())
                              .ReturnsAsync(products.Where(p => p.Rating > 3)
                                                    .OrderByDescending(p => p.CreatedDate)
                                                    .Take(30)
                                                    .ToList());
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult? result = await productController.GetHighRatings();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetHighRatings_BadRequest()
        {
            //ARRANGE
            //List of low rating products (rating <= 3)
            List<Product>? products = null;
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetHighRatingAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult? result = await productController.GetHighRatings();
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetAll_Success()
        {
            //ARRANGE
            //List of products
            List<Product> products = new()
            {
                new Product(){Id = 1, Name = "Product 1", Ratings = new List<Rating>()},
                new Product(){Id = 2, Name = "Product 2", Ratings = new List<Rating>()},
                new Product(){Id = 3, Name = "Product 3", Ratings = new List<Rating>()},
                new Product(){Id = 4, Name = "Product 4", Ratings = new List<Rating>()},
                new Product(){Id = 5, Name = "Product 5", Ratings = new List<Rating>()}
            };
            //Expected DTOs
            List<ProductDTO> expected = new()
            {
                new ProductDTO(){Id = 1, Name = "Product 1", Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 2, Name = "Product 2", Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 3, Name = "Product 3", Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 4, Name = "Product 4", Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 5, Name = "Product 5", Ratings = new List<RatingDTO>()}
            };
            //Arranging Service
            _productServiceMoq.Setup(p => p.GetAllAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.GetAll() as JsonResult;
            List<ProductDTO>? productDTO = (List<ProductDTO>?)result?.Value;
            Assert.NotNull(result);
            Assert.NotNull(productDTO);
            Assert.Equivalent(expected, productDTO);
        }

        [Fact]
        public async Task GetAll_NotFound()
        {
            //ARRANGE
            //List of products
            List<Product> products = new();

            //Arranging Service
            _productServiceMoq.Setup(p => p.GetAllAsync()).ReturnsAsync(products);
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult result = await productController.GetAll();
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("Category 1", 1, 2)]
        [InlineData("Category 1", 2, 2)]
        [InlineData("Category 1", 1, 3)]
        [InlineData("Category 1", 1, 4)]
        [InlineData("Category 2", 1, 2)]
        public async Task GetByCategory_Success(string categoryName, int page, int limit)
        {
            //ARRANGE
            //list of categories
            List<Category> categories = new()
            {
                new Category {Name = "Category 1"},
                new Category {Name = "Category 2"}
            };

            //list of products
            List<Product> products = new()
            {
                new Product(){Id = 1, Name = "Product 1", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 2, Name = "Product 2", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 3, Name = "Product 3", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 4, Name = "Product 4", Category = categories[1], Ratings = new List<Rating>()},
                new Product(){Id = 5, Name = "Product 5", Category = categories[1], Ratings = new List<Rating>()}
            };
            //expected DTOs
            List<ProductDTO> expected = new()
            {
                new ProductDTO(){Id = 1, Name = "Product 1", CategoryName = categories[0].Name, Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 2, Name = "Product 2", CategoryName = categories[0].Name, Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 3, Name = "Product 3", CategoryName = categories[0].Name, Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 4, Name = "Product 4", CategoryName = categories[1].Name, Ratings = new List<RatingDTO>()},
                new ProductDTO(){Id = 5, Name = "Product 5", CategoryName = categories[1].Name, Ratings = new List<RatingDTO>()}
            };
            //Arranging Service
            int realPage = page;
            int realLimit = limit;
            if (realPage > 0) realPage--;
            int count = products.Where(p => p.Category.Name == categoryName).ToList().Count;
            if (count < limit)
            {
                realPage = 0;
                realLimit = count;
            }
            _productServiceMoq.Setup(p => p.GetByCategoryAsync(categoryName, page, limit))
                              .ReturnsAsync(products.Where(p => p.Category.Name == categoryName)
                                                    .Skip(realPage * realLimit)
                                                    .Take(realLimit)
                                                    .ToList());
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.GetByCategory(categoryName, page, limit) as JsonResult;
            List<ProductDTO>? productDTO = (List<ProductDTO>?)result?.Value;
            Assert.NotNull(result);
            Assert.NotNull(productDTO);
            Assert.NotEmpty(productDTO);
            Assert.Equivalent(expected.Where(p => p.CategoryName == categoryName)
                                      .Skip(realPage * realLimit)
                                      .Take(realLimit), productDTO);
        }

        [Theory]
        [InlineData("Category 2", 1, 2)]
        [InlineData("Category 2", 2, 2)]
        [InlineData("Category 2", 1, 3)]
        [InlineData("Category 2", 1, 4)]
        public async Task GetByCategory_NotFound(string categoryName, int page, int limit)
        {
            //ARRANGE
            //list of categories
            List<Category> categories = new()
            {
                new Category {Name = "Category 1"},
                new Category {Name = "Category 2"}
            };

            //list of products
            List<Product> products = new()
            {
                new Product(){Id = 1, Name = "Product 1", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 2, Name = "Product 2", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 3, Name = "Product 3", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 4, Name = "Product 4", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 5, Name = "Product 5", Category = categories[0], Ratings = new List<Rating>()}
            };

            //Arranging Service
            int realPage = page;
            int realLimit = limit;
            if (realPage > 0) realPage--;
            int count = products.Where(p => p.Category.Name == categoryName).ToList().Count;
            if (count < limit)
            {
                realPage = 0;
                realLimit = count;
            }
            _productServiceMoq.Setup(p => p.GetByCategoryAsync(categoryName, page, limit))
                              .ReturnsAsync(new List<Product>());
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            ActionResult? result = await productController.GetByCategory(categoryName, page, limit);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task AdminGetByID_Success(int id)
        {
            //ARRANGE
            //list of categories
            List<Category> categories = new()
            {
                new Category {Name = "Category 1"},
                new Category {Name = "Category 2"}
            };
            //list of products
            List<Product> products = new()
            {
                new Product(){Id = 1, Name = "Product 1", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 2, Name = "Product 2", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 3, Name = "Product 3", Category = categories[0], Ratings = new List<Rating>()},
                new Product(){Id = 4, Name = "Product 4", Category = categories[1], Ratings = new List<Rating>()},
                new Product(){Id = 5, Name = "Product 5", Category = categories[1], Ratings = new List<Rating>()}
            };

            //expected DTOs
            List<ProductADTO> expected = new()
            {
                new ProductADTO(){Id = 1, Name = "Product 1", CategoryName = categories[0].Name},
                new ProductADTO(){Id = 2, Name = "Product 2", CategoryName = categories[0].Name},
                new ProductADTO(){Id = 3, Name = "Product 3", CategoryName = categories[0].Name},
                new ProductADTO(){Id = 4, Name = "Product 4", CategoryName = categories[1].Name},
                new ProductADTO(){Id = 5, Name = "Product 5", CategoryName = categories[1].Name}
            };
            _productServiceMoq.Setup(p => p.GetByIDAsync(id)).ReturnsAsync(products.FirstOrDefault(p => p.Id == id));
            //Arranging Controller
            ProductController productController = new ProductController(_productServiceMoq.Object,
                                                                        _categoryServiceMoq.Object,
                                                                        _brandServiceMoq.Object);

            //ACT
            JsonResult? result = await productController.AdminGetByID(id) as JsonResult;
            ProductADTO? productADTO = (ProductADTO?)result?.Value;
            Assert.NotNull(result);
            Assert.Equivalent(expected.FirstOrDefault(p => p.Id == id),
                              productADTO);
        }
    }
}
