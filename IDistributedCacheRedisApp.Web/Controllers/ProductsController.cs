using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "asya", options); 
            await _distributedCache.SetStringAsync("surname", "guldemir", options);
            return View();
        }

        public IActionResult Show()
        {

            string name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();

        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();

        }
    }
}
