using StackExchange.Redis;
using System.Text.Json;

namespace EMS.Services
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var data = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(data))
            {
                return JsonSerializer.Deserialize<T>(data);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var exits = _cacheDb.KeyExists(key);
            if (exits)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T data, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(data), expiryTime);
        }
    }
}
