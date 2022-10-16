using EcommerceAPI.Data;
using EcommerceAPI.Models;
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

        public ProductController(EcommerceDbContext context)
        {
            _context = context;
            _productService = new ProductService(_context);
        }

        [HttpGet]
        //[ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllAsync();
            if(products != null)
            {
                if(products.Count > 0) return Json(products);
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
                if(products.Count > 0) return Json(products);
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        //[ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetByCategory(int id)
        {
            List<Product> products = await _productService.GetByCategoryAsync(id);
            if (products != null)
            {
                if(products.Count > 0) return Json(products);
                return NotFound();
            }

            return BadRequest();
        }
    }
}
