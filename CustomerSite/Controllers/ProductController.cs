using Ecommerce.DTO.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.CustomerSite.Controllers
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
                ProductDTO? product = JsonConvert.DeserializeObject<ProductDTO>(content);
                return View(product);
            }
            ViewData["response"] = response.StatusCode;
            return View(null);
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByCategory(string categoryName, int page = 1, int limit = 6)
        {
            ViewData["Title"] = categoryName;
            var response = await _httpClient.GetAsync($"Product/GetByCategory/?categoryName={categoryName}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<ProductDTO>? products = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
                response = await _httpClient.GetAsync($"Category/CountProducts/?categoryName={categoryName}");
                int pages = await response.Content.ReadFromJsonAsync<int>();
                pages = (int)Math.Ceiling((double)pages / limit);
                ViewBag.pages = pages;
                ViewBag.page = page;
                ViewBag.limit = limit;
                return View(products);
            }
            ViewData["response"] = response.StatusCode;
            return View(null);
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByBrand(string brandName, int page = 1, int limit = 6)
        {
            ViewData["Title"] = brandName;
            var response = await _httpClient.GetAsync($"Product/GetByBrand/?brandName={brandName}&page={page}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<ProductDTO>? products = JsonConvert.DeserializeObject<List<ProductDTO>>(content);
                response = await _httpClient.GetAsync($"Brand/CountProducts/?brandName={brandName}");
                int pages = await response.Content.ReadFromJsonAsync<int>();
                pages = (int)Math.Ceiling((double)pages / limit);
                ViewBag.pages = pages;
                ViewBag.page = page;
                ViewBag.limit = limit;
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
                    new RatingDTO
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
