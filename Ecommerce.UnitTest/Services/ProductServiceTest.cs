﻿using Ecommerce.API.Data;
using Ecommerce.API.Services;
using Ecommerce.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ecommerce.API.UnitTest.Services
{
    public class ProductServiceTest : IDisposable
    {
        private DbContextOptions<EcommerceDbContext> _options;
        private EcommerceDbContext _context;
        private List<Category> _categories;
        private List<Brand> _brands;
        private List<Product> _products;
        private List<Rating> _ratings;
        private List<IdentityUser> _users;

        public ProductServiceTest()
        {
            //Implement in memory dbcontext for service testing
            _options = new DbContextOptionsBuilder<EcommerceDbContext>().UseInMemoryDatabase("ProductTestDB").Options;
            _context = new EcommerceDbContext(_options);

            //Create fake data
            _categories = new()
            {
                new Category(){Name="Cat 1"},
                new Category(){Name="Cat 2"},
                new Category(){Name="Cat 3"},
            };

            _brands = new()
            {
                new Brand(){Name="Brand 1"},
                new Brand(){Name="Brand 2"},
                new Brand(){Name="Brand 3"},
            };

            _users = new()
            {
                new IdentityUser(){Id = "user1"},
                new IdentityUser(){Id = "user2"},
                new IdentityUser(){Id = "user3"}

            };

            _products = new()
            {
                new Product(){Id=1, Name="Product 1", Category = _categories[0], Brand = _brands[0]},
                new Product(){Id=2, Name="Product 2", Category = _categories[0], Brand = _brands[1]},
                new Product(){Id=3, Name="Product 3", Category = _categories[1], Brand = _brands[2]},
                new Product(){Id=4, Name="Product 4", Category = _categories[1], Brand = _brands[0]},
                new Product(){Id=5, Name="Product 5", Category = _categories[1], Brand = _brands[1]},
                new Product(){Id=6, Name="Product 6", Category = _categories[2], Brand = _brands[2]},
            };

            _ratings = new()
            {
                new Rating(){Id=11, Product = _products[0], User=_users[0]},
                new Rating(){Id=12, Product = _products[0], User=_users[1]},
                new Rating(){Id=13, Product = _products[1], User=_users[0]},
                new Rating(){Id=14, Product = _products[1], User=_users[2]},
                new Rating(){Id=15, Product = _products[2], User=_users[1]}
            };

            foreach (Product p in _products) p.Ratings = _ratings.Where(r => r.Product.Id == p.Id).ToList();
            _context.Database.EnsureDeleted();
            _context.Categories.AddRange(_categories);
            _context.Brands.AddRange(_brands);
            _context.Users.AddRange(_users);
            _context.Ratings.AddRange(_ratings);
            _context.Products.AddRange(_products);
            _context.SaveChanges();

        }

        [Fact]
        public void GetAll_Success()
        {
            //ARRANGE
            ProductService productService = new ProductService(_context);
            
            //ACT
            List<Product>? result = productService.GetAllAsync().Result;
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equivalent(_products, result);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        [InlineData(0, 6)]
        [InlineData(0, 10)]
        public void GetPage_Success(int page, int limit)
        {
            //ARRANGE
            ProductService productService = new ProductService(_context);
            //ACT
            List<Product>? result = productService.GetPageAsync(page, limit).Result;
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            int count = _context.Products.Count();
            if (page > 0) page--;
            if (count < limit)
            {
                page = 0;
                limit = count;
            }
            Assert.Equivalent(_products.Skip(page*limit).Take(limit), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void GetByID_Success(int id)
        {
            //ARRANGE
            ProductService productService = new ProductService(_context);

            //ACT
            Product? result = productService.GetByIDAsync(id).Result;
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equivalent(_products.SingleOrDefault(p => p.Id == id), result);
        }

        //Clean up after test
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}