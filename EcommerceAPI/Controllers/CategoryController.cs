using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly EcommerceDbContext _context;
        private CategoryService _categoryService;

        public CategoryController(EcommerceDbContext context)
        {
            _context = context;
            _categoryService = new CategoryService(_context);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories()
        {
            List<Category> categories = await _categoryService.GetAllAsync();
            if (categories != null) return Json(categories);
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            Category category = await _categoryService.GetByIDAsync(id);
            return category != null ? Json(category) : BadRequest();
        }
    }
}
