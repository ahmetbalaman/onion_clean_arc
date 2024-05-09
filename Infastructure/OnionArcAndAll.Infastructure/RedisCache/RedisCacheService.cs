using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnionArcAndAll.Application.Interfaces.RedisCache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Infastructure.RedisCache
{
    internal class RedisCacheService : IRedisCacheService
    {
        private readonly ConnectionMultiplexer redisConnection;
        private readonly IDatabase database;
        private readonly RedisCacheSettings settings;

        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            settings = options.Value;
            var opt = ConfigurationOptions.Parse(settings.ConnectionString);
            redisConnection = ConnectionMultiplexer.Connect(opt);
            database = redisConnection.GetDatabase();

        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await database.StringGetAsync(key);
            if (value.HasValue)
                return JsonConvert.DeserializeObject<T>(value);
            return default;
        }
        public async Task<T> SetAsync<T>(string key, T value, DateTime? expirationTime = null)
        {
            TimeSpan timeUnitExpiration;
            //if(expirationTime is not null)
            timeUnitExpiration = expirationTime.Value - DateTime.Now;

            await database.StringSetAsync(key, JsonConvert.SerializeObject(value), timeUnitExpiration);
            throw new NotImplementedException();
        }
    }
}
