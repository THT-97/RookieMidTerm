using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerSite.Controllers
{
    public class ProductController : Controller
    {
        private HttpClient _httpClient;

        public ProductController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7171/api/");
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetailsAsync(int id)
        {
            var response = await _httpClient.GetAsync("Product/GetById/?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(content);
            return View(product);
        }
    }
}
