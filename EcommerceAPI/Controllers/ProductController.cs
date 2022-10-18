﻿using AutoMapper;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using EcommerceClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext _context;
        private ProductService _productService;
        private IMapper _mapper;

        public ProductController(EcommerceDbContext context)
        {
            _context = context;
            _productService = new ProductService(_context);
            //create mapper with custom config
            _mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<Product, ProductDTO>()
                   //for CategoryName (Category object => CategoryName)
                   .ForMember(dto => dto.CategoryName, src => src.MapFrom(ent => ent.Category.Name))
                   //for BrandName (Brand object => BrandName)
                   .ForMember(dto => dto.BrandName, src => src.MapFrom(ent => ent.Brand.Name))
            ).CreateMapper();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllAsync();
            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(products); ;
            if (products != null)
            {
                if (productDTOs.Count > 0) return Json(productDTOs);
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetNewProducts()
        {
            List<Product> products = await _productService.GetNewAsync();
            if (products != null)
            {
                if(products.Count > 0) return Json(_mapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        //[ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetByCategory(string categoryName)
        {
            List<Product> products = await _productService.GetByCategoryAsync(categoryName);
            if (products != null)
            {
                if(products.Count > 0) return Json(_mapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }
    }
}
