using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly EcommerceDbContext _context;
        private ProductService _productService;
        public ProductController(EcommerceDbContext dbContext)
        {
            _context = dbContext;
            _productService = new ProductService(dbContext);
        }

        // GET: ProductController
        [HttpGet(Name = "GetAllProducts")]
        public ActionResult GetAllProducts()
        {
            List<Product> products = _productService.GetAllProducts();
            if(products != null) return Json(products);
            return BadRequest();
        }
    }
}
