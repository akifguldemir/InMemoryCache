using InMemory.Web.Models;
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

                //cache in neden silindiğini loglar
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _cache.Set("callback", $"Key: {key} Value: {value} Reason: {reason}");
                });

                _cache.Set<string>("zaman", DateTime.Now.ToString(), options);

                Product product = new Product()
                {
                    Id = 1,
                    Name = "Kalem",
                    Price = 10
                };

                _cache.Set<Product>("product:1", product, options); //Product nesnesini cache'e ekle
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
            _cache.TryGetValue("callback", out string callback);

            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            ViewBag.product = _cache.Get<Product>("product:1");
            return View();
        }
    }
}
