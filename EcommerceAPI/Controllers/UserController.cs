using AutoMapper;
using Ecommerce.API.ServiceInterfaces;
using Ecommerce.Data.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private IMapper _mapper;

        public UserController(IUserService userService)
        {
            _userService = userService;
            _mapper = new MapperConfiguration(cfg =>
                //base mapping config
                cfg.CreateMap<IdentityUser, UserDTO>()
                .ForMember(dto => dto.Id, src => src.MapFrom(ent => ent.Id))
            ).CreateMapper();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<IdentityUser> users = await _userService.GetAllAsync();
            if(users != null)
            {
                if (users.Count > 0) return Json(_mapper.Map<List<IdentityUser>, List<UserDTO>>(users));
                return NotFound();
            }
            return BadRequest();
        }
    }
}
