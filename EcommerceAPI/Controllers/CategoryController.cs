using AutoMapper;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()
                                                           .ReverseMap()).CreateMapper();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Category>? categories = await _categoryService.GetAllAsync();
            if (categories != null)
            {
                if (categories.Count > 0) return Json(_mapper.Map<List<CategoryDTO>>(categories));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<int> Count()
        {
            return await _categoryService.CountAsync();
        }

        [HttpGet]
        public async Task<ActionResult> GetByID(int id)
        {
            Category category = await _categoryService.GetByIDAsync(id);
            return category != null ? Json(_mapper.Map<CategoryDTO>(category)) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> GetByName(string name)
        {
            Category? category = await _categoryService.GetByNameAsync(name);
            return category != null ? Json(_mapper.Map<List<CategoryDTO>>(category)) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> GetPage(int page = 0, int limit = 6)
        {
            List<Category>? categories = await _categoryService.GetPageAsync(page, limit);
            if (categories != null)
            {
                if (categories.Count > 0) return Json(_mapper.Map<List<CategoryDTO>>(categories));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> CountProducts(string categoryName)
        {
            int count = await _categoryService.CountProductsAsync(categoryName);
            return count > -1 ? Json(count) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryDTO category)
        {
            if (ModelState.IsValid) return await _categoryService.CreateAsync(_mapper.Map<Category>(category));
            return BadRequest();
        }

        [HttpPut()]
        public async Task<ActionResult> Update(int id, CategoryDTO category)
        {
            return await _categoryService.UpdateAsync(id, _mapper.Map<Category>(category));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
