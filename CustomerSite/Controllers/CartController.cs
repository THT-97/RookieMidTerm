using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.CustomerSite.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
