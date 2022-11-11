using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ecommerce.CustomerSite.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
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
                //If login successful, save username and token to cookies
                string token = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("Username", name, new CookieOptions{ Expires = DateTime.Now.AddHours(12) });
                Response.Cookies.Append("Token", token, new CookieOptions { Expires = DateTime.Now.AddHours(12) });
                //Get user role and save to cookies
                response = await _httpClient.GetAsync($"Auth/GetRoles?username={name}");
                string roles = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("Roles", roles, new CookieOptions { Expires = DateTime.Now.AddHours(12) });
                //Redirect to previous page (not sign in or register page)
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
            Response.Cookies.Delete("Roles");
            if (prevPage == null
                || prevPage == "https://localhost:7091/User/SignIn"
                || prevPage == "https://localhost:7091/User/Register")
                return RedirectToAction("Index", "Home", null);
            return Redirect(prevPage);
            
        }
    }
}
