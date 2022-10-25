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
        private ProductService _productService;
        private IMapper _outmapper;
        private IMapper _inmapper;

        public ProductController(EcommerceDbContext context)
        {
            _context = context;
            _productService = new ProductService(_context);
            //create mapper with custom config
            _outmapper = new MapperConfiguration(cfg =>
            {
                //base mapping config
                cfg.CreateMap<Product, ProductDTO>()
                    //config for Category entity
                   .ForMember(dto => dto.CategoryName, src => src.MapFrom(ent => ent.Category.Name))
                   //config for Brand entity
                   .ForMember(dto => dto.BrandName, src => src.MapFrom(ent => ent.Brand.Name));
                //specific mapping config for Rating collection
                cfg.CreateMap<Rating, RatingDTO>()
                   //config for rating points
                   .ForMember(dto => dto.Rate, src => src.MapFrom(ent => ent.Points))
                   //config for User entity
                   .ForMember(dto => dto.UserEmail, src => src.MapFrom(ent => ent.User.Email));
            }).CreateMapper();

            _inmapper = new MapperConfiguration(cfg =>
            {
                //base mapping config
                cfg.CreateMap<ProductDTO, Product>()
                   //config for Category entity
                   .ForMember(obj => obj.Category, src => src.MapFrom(dto => _context.Categories.FirstOrDefault(c => c.Name == dto.CategoryName)))
                   .ForMember(obj => obj.Brand, src => src.MapFrom(dto => _context.Brands.FirstOrDefault(b => b.Name == dto.BrandName)));
            }).CreateMapper();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Product> products = await _productService.GetAllAsync();
            List<ProductDTO> productDTOs = _outmapper.Map<List<ProductDTO>>(products); ;
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
                if(products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
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
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
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
                if(products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetByID(int id)
        {
            Product product = await _productService.GetByIDAsync(id);
            if (product != null) return Json(_outmapper.Map<ProductDTO>(product));
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                return await _productService.CreateAsync(_inmapper.Map<Product>(product));
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Rate(RatingDTO rating)
        {
            try
            {
                return await _productService.RateAsync(rating);
            }

            catch { return new BadRequestResult(); }
        }

    }
}
