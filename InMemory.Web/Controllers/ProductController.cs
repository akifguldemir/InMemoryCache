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
            //1.yol
            //if(String.IsNullOrEmpty(_cache.Get<string>("zaman")))
            //{
            //    _cache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            //2.yol
            if(!_cache.TryGetValue("zaman", out string zamancache))
            {
                //Cache süresi
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10); AbsoluteExpiration örneği

                // SlidingExpiration örneği
                //options.SlidingExpiration = TimeSpan.FromSeconds(10);

                //SlidingExpiration kullanıclacağında best practices olarak AbsoluteExpiration da verilmeli..1 dk ye kadar veri alınınca 10 sn eklenir
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);

                //cache önceliği
                options.Priority = CacheItemPriority.Normal; //Normal öncelik - Hİgh,Low, NeverRemove

                _cache.Set<string>("zaman", DateTime.Now.ToString(), options);
            }

            return View();
        }

        public IActionResult Get()
        {

            //_cache.Remove("zaman"); Remove
            //_cache.GetOrCreate("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //}); yoksa olusturur

            _cache.TryGetValue("zaman", out string zamancache);

            ViewBag.zaman = zamancache;
            return View();
        }
    }
}
