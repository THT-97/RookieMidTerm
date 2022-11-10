using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.CustomerSite.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
        private string? _token;
        public UserController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("client");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(string name, string password, string prevPage)
        {
            LoginDTO login = new LoginDTO { username = name, password = password };
            var response = await _httpClient.PostAsJsonAsync("Auth/Register", login);
            if (response.IsSuccessStatusCode)
            {
                return await SignInAsync(name, password, prevPage);
            }
            return new BadRequestResult();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync(string name, string password, string prevPage)
        {
            LoginDTO login = new LoginDTO { username = name, password = password};
            var response = await _httpClient.PostAsJsonAsync("Auth/SignIn", login);
            if (response.IsSuccessStatusCode)
            {
                _token = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("Username", name, new CookieOptions{ Expires = DateTime.Now.AddHours(12) });
                Response.Cookies.Append("Token", _token, new CookieOptions { Expires = DateTime.Now.AddHours(12) });
                if (prevPage == null
                || prevPage == "https://localhost:7091/User/SignIn"
                || prevPage == "https://localhost:7091/User/Register")
                    return RedirectToAction("Index", "Home", null);
                return Redirect(prevPage);
            }
            ViewBag.error = "Incorrect username or password";
            return View();
        }

        [HttpPost]
        public IActionResult SignOutAsync(string? prevPage)
        {
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Token");
            if (prevPage == null
                || prevPage == "https://localhost:7091/User/SignIn"
                || prevPage == "https://localhost:7091/User/Register")
                return RedirectToAction("Index", "Home", null);
            return Redirect(prevPage);
            
        }
    }
}
