using AutoMapper;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private IBrandService _brandService;
        private IMapper _outmapper;
        private IMapper _inmapper;

        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
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

                //config for DTO used by admin site
                cfg.CreateMap<Product, ProductADTO>()
                  //config for Category entity
                  .ForMember(dto => dto.CategoryName, src => src.MapFrom(ent => ent.Category.Name))
                  //config for Brand entity
                  .ForMember(dto => dto.BrandName, src => src.MapFrom(ent => ent.Brand.Name));
            }).CreateMapper();

            _inmapper = new MapperConfiguration(cfg =>
            {
                //base mapping config
                cfg.CreateMap<ProductDTO, Product>()
                   //config for Category entity
                   .ForMember(obj => obj.Category, src => src.MapFrom(dto => _categoryService.GetByNameAsync(dto.CategoryName).Result))
                   .ForMember(obj => obj.Brand, src => src.MapFrom(dto => _brandService.GetByNameAsync(dto.BrandName).Result));
            }).CreateMapper();
        }

        [HttpGet]
        public async Task<int> Count()
        {
            return await _productService.CountAsync();
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> GetAll()
        {
            List<Product>? products = await _productService.GetAllAsync();
            List<ProductDTO> productDTOs = _outmapper.Map<List<ProductDTO>>(products);
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
            List<Product>? products = await _productService.GetNewAsync();
            if (products != null)
            {
                
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetHighRatings()
        {
            List<Product>? products = await _productService.GetHighRatingAsync();
            if (products != null)
            {
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetByCategory(string categoryName, int page=0, int limit=6)
        {
            List<Product>? products = await _productService.GetByCategoryAsync(categoryName, page, limit);
            if (products != null)
            {
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetByBrand(string brandName, int page = 0, int limit = 6)
        {
            List<Product>? products = await _productService.GetByBrandAsync(brandName, page, limit);
            if (products != null)
            {
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductDTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetByID(int id)
        {
            Product? product = await _productService.GetByIDAsync(id);
            if (product != null) return Json(_outmapper.Map<ProductDTO>(product));
            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> AdminGetByID(int id)
        {
            Product? product = await _productService.GetByIDAsync(id);
            if (product != null) return Json(_outmapper.Map<ProductADTO>(product));
            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> GetPage(int page=0, int limit=6)
        {
            List<Product>? products = await _productService.GetPageAsync(page, limit);
            if (products != null)
            {
                if (products.Count > 0) return Json(_outmapper.Map<List<ProductADTO>>(products));
                return NotFound();
            }

            return BadRequest();
        }


        [HttpPost]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                return await _productService.CreateAsync(_inmapper.Map<Product>(product));
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Update(int id, ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                return await _productService.UpdateAsync(id, _inmapper.Map<Product>(product));
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "SysAdmin")]
        public async Task<ActionResult> Delete(int id)
        {
            return await _productService.DeleteAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
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
