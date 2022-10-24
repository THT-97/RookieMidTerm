using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerSite.Controllers
{
    public class ProductController : Controller
    {
        private HttpClient _httpClient;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("client");
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetailsAsync(int id)
        {
            var response = await _httpClient.GetAsync("Product/GetById/?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(content);
                return View(product);
            }
            ViewData["response"] = response.StatusCode;
            return View(null);
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(string categoryName)
        {
            ViewData["Title"] = categoryName;
            var response = await _httpClient.GetAsync("Product/GetByCategory/?categoryName=" + categoryName);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<ProductDTO> products = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
                return View(products);
            }
            ViewData["response"] = response.StatusCode;
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> RateAsync(int productId, byte rate, string comment)
        {
            if (User.Identity.Name != null)
            {
                var response = await _httpClient.PostAsJsonAsync("Product/Rate", 
                    new ProductRateDTO
                    {
                        ProductId = productId,
                        UserEmail = User.Identity.Name,
                        Rate = rate,
                        Date = DateTime.Now,
                        Comment = comment
                    });

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProductDetails", new { id = productId });
                }
            }

            return new BadRequestResult();
        }
    }
}
