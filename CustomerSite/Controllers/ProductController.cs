using Ecommerce.DTO.DTOs;
using Ecommerce.DTO.Enum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

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
                if (product.Status != (byte)CommonStatus.NotAvailable
                    && product.Status != (byte)ProductStatus.Suspended)
                {
                    return View(product);
                }
                ViewData["response"] = HttpStatusCode.Forbidden;
                return View();
            }
            ViewData["response"] = response.StatusCode;
            return View();
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
            return View();
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RateAsync(int productId, byte rate, string comment)
        {
            if (Request.Cookies["UserName"] != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
                var response = await _httpClient.PostAsJsonAsync("Product/Rate",
                    new RatingDTO
                    {
                        ProductId = productId,
                        UserEmail = Request.Cookies["UserName"],
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
