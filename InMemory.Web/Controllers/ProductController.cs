using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _cache;
        public ProductController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            _cache.Set<string>("zaman", DateTime.Now.ToString());
            return View();
        }

        public IActionResult Get()
        {
            ViewBag.zaman = _cache.Get<string>("zaman");
            return View();
        }
    }
}
