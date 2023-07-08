using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;

namespace NamanApi.Services
{
    public static class SecondLevelCache
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private const int AbsoluteExpirationSeconds = 10;


        public static string Sha256Hash(string queryString)
        {
            // Create a SHA256 hash object.
            var sha256 = SHA256.Create();

            // Calculate the hash of the string.
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(queryString));

            // Convert the hash bytes to a string.
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
        private static string GetCacheKey(IQueryable query)
        {
            var queryString = query.ToQueryString();
            var hash = Sha256Hash(queryString);
            return hash;
        }

        public static List<T> FromCache<T>(this IQueryable<T> query)
        {
            var key = GetCacheKey(query);

            var result = _cache.GetOrCreate(key, cache =>
            {
                cache.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(AbsoluteExpirationSeconds);
                return query.ToList();
            }) ?? new List<T>();

            return result;
        }

        public static async Task<List<T>> FromCacheAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            var key = GetCacheKey(query);

            var result = await _cache.GetOrCreateAsync(key, cache =>
            {
                cache.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(AbsoluteExpirationSeconds);
                return query.ToListAsync(cancellationToken);
            }) ?? new List<T>();

            return result;
        }

        public static void Clear()
        {
            _cache.Dispose();
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
    }
}
