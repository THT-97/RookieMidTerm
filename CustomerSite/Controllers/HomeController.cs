using Ecommerce.CustomerSite.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Ecommerce.CustomerSite.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _httpClient;
        private HttpResponseMessage? _response;
        private string? _content;
        private string? _token;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("client");
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name != null && HttpContext.Session.GetString("token") == null)
            {
                _response = await _httpClient.PostAsJsonAsync("Auth/Authenticate", User.Identity.Name);
                _token = await _response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("token", _token);
            }
            List<ProductDTO>? highRatings;
            List<ProductDTO>? newProducts;
            List<CategoryDTO>? categories;
            List<BrandDTO>? brands;

            //Call API Controller to get list of categories
            _response = await _httpClient.GetAsync("Category/GetAll");
            _content = await _response.Content.ReadAsStringAsync();
            categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(_content);

            //Call again to get brand
            _response = await _httpClient.GetAsync("Brand/GetAll");
            _content = await _response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<BrandDTO>>(_content);

            //Call again to get list of all products
            _response = await _httpClient.GetAsync("Product/GetHighRatings");
            _content = await _response.Content.ReadAsStringAsync();
            highRatings = JsonConvert.DeserializeObject<List<ProductDTO>>(_content);

            //Call again to get new products
            _response = await _httpClient.GetAsync("Product/GetNew");
            _content = await _response.Content.ReadAsStringAsync();
            newProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(_content);

            //Transfer data to ViewBag
            ViewBag.categories = categories;
            ViewBag.brands = brands;
            ViewBag.highRatings = highRatings;
            ViewBag.newProducts = newProducts;

            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}