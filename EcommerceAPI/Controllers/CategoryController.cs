using AutoMapper;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
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
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByID(int id)
        {
            Category category = await _categoryService.GetByIDAsync(id);
            return category != null ? Json(_mapper.Map<CategoryDTO>(category)) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            if (ModelState.IsValid) return await _categoryService.CreateAsync(category);
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Category category)
        {
            return await _categoryService.UpdateAsync(id, category);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await _categoryService.DeleteAsync(id);
        }
    }
}
