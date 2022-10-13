using CustomerSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CustomerSite.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient httpClient;   
        private readonly ILogger<HomeController> _logger;
        private List<Product> products;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            //Call to API Controller to get list of all products
            var response = await httpClient.GetAsync("https://localhost:7171/Product");
            var content = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<Product>>(content);
            return View(products);
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