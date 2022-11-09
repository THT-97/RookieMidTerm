using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.CustomerSite.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
        public UserController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("client");
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync(string name, string password)
        {
            LoginDTO login = new LoginDTO { username = name, password = password};
            var response = await _httpClient.PostAsJsonAsync("Auth/SignIn", login);
            return View();
        }
    }
}
