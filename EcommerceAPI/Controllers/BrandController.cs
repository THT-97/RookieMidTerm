using AutoMapper;
using Ecommerce.API.Data;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.API.Services;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandController : Controller
    {

        private IBrandService _brandService;
        private IMapper _mapper;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Brand, BrandDTO>()).CreateMapper();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Brand>? brands = await _brandService.GetAllAsync();
            if (brands != null)
            {
                if (brands.Count > 0) return Json(_mapper.Map<List<BrandDTO>>(brands));
                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByID(int id)
        {
            Brand? brand = await _brandService.GetByIDAsync(id);
            return brand != null ? Json(_mapper.Map<BrandDTO>(brand)) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> CountProducts(string brandName)
        {
            int count = await _brandService.CountProductsAsync(brandName);
            return count != -1 ? Json(count) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Brand brand)
        {
            if (ModelState.IsValid) return await _brandService.CreateAsync(brand);
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Brand brand)
        {
            return await _brandService.UpdateAsync(id, brand);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return await _brandService.DeleteAsync(id);
        }
    }
}
