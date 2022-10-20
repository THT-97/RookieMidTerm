using AutoMapper;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using EcommerceAPI.Data;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext _context;
        private IProductRepository _productService;
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
        public async Task<ActionResult> GetAll()
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
        public async Task<ActionResult> GetNew()
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
        public async Task<ActionResult> GetHighRatings()
        {
            List<Product> products = await _productService.GetHighRatingAsync();
            if (products != null)
            {
                if (products.Count > 0) return Json(_mapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<ActionResult> GetByID(int id)
        {
            Product product = await _productService.GetByIDAsync(id);
            if (product != null) return Json(_mapper.Map<ProductDTO>(product));
            return NotFound();
        }
    }
}
