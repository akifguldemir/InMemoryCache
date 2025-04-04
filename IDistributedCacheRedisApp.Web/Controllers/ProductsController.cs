using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            //Basic datas
            //_distributedCache.SetString("name", "asya", options); 
            //await _distributedCache.SetStringAsync("surname", "guldemir", options);

            Product product = new Product()
            {
                Id = 1,
                Name = "Product 1",
                Price = 100
            };

            //as json
            string jsonProduct = JsonConvert.SerializeObject(product);

            //as byte
            Byte[] jsonProductByte = System.Text.Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("product:1", jsonProductByte, options);

            await _distributedCache.SetStringAsync("product:2", jsonProduct, options);
            return View();
        }

        public IActionResult Show()
        {

            //string name = _distributedCache.GetString("name");
            //ViewBag.name = name;

            string jsonProduct = _distributedCache.GetString("product:1");

            Byte[] jsonProductByte = _distributedCache.Get("product:1");

            string jsonProductByteString = System.Text.Encoding.UTF8.GetString(jsonProductByte);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;

            return View();

        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();

        }

        public IActionResult Image()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "car.jpg");

            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

            _distributedCache.Set("resim", imageData, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10)
            });

            return View();
        }

        public IActionResult Imageurl()
        {
            byte[] imageData = _distributedCache.Get("resim");

            return File(imageData, "image/jpeg");
        }
    }
}
