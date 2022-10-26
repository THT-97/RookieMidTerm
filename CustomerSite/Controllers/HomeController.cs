using CustomerSite.Models;
using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;

namespace CustomerSite.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _httpClient;
        private HttpResponseMessage _response;
        private string _content;
        private string _token;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("client");
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name != null && HttpContext.Session.GetString("token")==null)
            {
                _response = await _httpClient.PostAsJsonAsync("Auth/Authenticate", User.Identity.Name);
                _token = await _response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("token", _token);
            }
            List<ProductDTO> highRatings;
            List<ProductDTO> newProducts;
            List<CategoryDTO> categories;
            List<BrandDTO> brands;
            //Call API Controller to get list of categories
            var response = await _httpClient.GetAsync("Category/GetAll");
            var content = await response.Content.ReadAsStringAsync();
            categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
            //Call again to get brand
            response = await _httpClient.GetAsync("Brand/GetAll");
            content = await response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<BrandDTO>>(content);
            //Call again to get list of all products
            response = await _httpClient.GetAsync("Product/GetHighRatings");
            content = await response.Content.ReadAsStringAsync();
            highRatings = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
            //Call again to get new products
            response = await _httpClient.GetAsync("Product/GetNew");
            content = await response.Content.ReadAsStringAsync();
            newProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
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