using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext _context;
        private ProductService _productService;
        public ProductController(EcommerceDbContext dbContext)
        {
            _context = dbContext;
            _productService = new ProductService(dbContext);
        }

        [HttpGet(Name = "AllProducts")]
        //[ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllAsync();
            if(products != null) return Json(products);
            return BadRequest();
        }

        [HttpGet(Name = "NewProducts")]
        public async Task<ActionResult> GetNewProducts()
        {
            List<Product> products = await _productService.GetNewAsync();
            if (products != null) return Json(products);
            return BadRequest();
        }

        [HttpGet("{id}", Name = "ByCategory")]
        //[ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetByCategory(int id)
        {
            List<Product> products = await _productService.GetByCategoryAsync(id);
            if (products != null && products.Count>0) return Json(products);
            if (products.Count == 0) return NotFound();
            return BadRequest();
        }
    }
}
