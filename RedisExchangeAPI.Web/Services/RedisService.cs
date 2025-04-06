using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly string _redisHost;
        private readonly string _redisPort;

        private IDatabase db { get; set;  }

        public RedisService(IConfiguration configuration)
        {
            var _redisHost = configuration["Redis:Host"];
            var _redisPort = configuration["Redis:Port"];

        }

        public void Connect()
        {
            var redisConnectionString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
