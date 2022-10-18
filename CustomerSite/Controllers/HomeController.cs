using CustomerSite.Models;
using EcommerceClassLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CustomerSite.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _httpClient;   

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7171/api/");
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO> products;
            List<ProductDTO> newProducts;
            List<CategoryDTO> categories;
            //Call to API Controller to get list of all products
            var response = await _httpClient.GetAsync("Product/GetAllProducts");
            var content = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
            //Call again to get new products
            response = await _httpClient.GetAsync("Product/GetNewProducts");
            content = await response.Content.ReadAsStringAsync();
            newProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
            //Call again to get categories
            response = await _httpClient.GetAsync("Category/GetAllCategories");
            content = await response.Content.ReadAsStringAsync();
            categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
            //Transfer data to ViewBag
            ViewBag.categories = categories;
            ViewBag.products = products;
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