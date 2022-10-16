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
            if (categories != null)
            {
                if(categories.Count > 0) return Json(categories);
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            Category category = await _categoryService.GetByIDAsync(id);
            return category != null ? Json(category) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid) return await _categoryService.CreateAsync(category);
            return BadRequest();
        }
    }
}
