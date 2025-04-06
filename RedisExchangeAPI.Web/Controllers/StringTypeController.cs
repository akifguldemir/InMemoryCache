using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {

        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "John Doe");
            db.StringSet("age", 30);
            return View();
        }

        public IActionResult Show()
        {
            //Clidaki aynı metotlar

            var value = db.StringGet("name");

            //db.StringIncrement("age", 1);

            //Dönen sonucla ilgili işlem varsa, async
            //var age = db.StringIncrementAsync("age",10).Result;

            //Sonuca gerek yok async
            db.StringIncrementAsync("age", 10).Wait();

            //var age = db.StringDecrementAsync("age", 10).Result;
            if (value.HasValue) ViewBag.Name = value.ToString();

            return View();
        }
    }
}
