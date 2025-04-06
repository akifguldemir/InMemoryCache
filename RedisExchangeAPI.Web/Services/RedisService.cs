using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _redis;
        private readonly string _redisHost;
        private readonly string _redisPort;
        private readonly string _redisConnectionString;

        private IDatabase db { get; set;  }

        public RedisService(IConfiguration configuration)
        {
            _redisConnectionString = configuration["Redis:Configuration"];
        }

        public void Connect()
        {
            var redisConnectionString = $"{_redisConnectionString}";

            _redis = ConnectionMultiplexer.Connect(_redisConnectionString);
        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
