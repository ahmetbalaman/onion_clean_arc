using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Interfaces.RedisCache
{
    public interface IRedisService
    {
        Task<T> GetAsync<T>(string key);
        Task<T> SetAsync<T>(string key, T value, DateTime? expirationTime = null);

    }
}
