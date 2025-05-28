using RestrantApplication.Core.Services;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestrantApplication.EF.Services
{
    /// <summary>
    /// A service for interacting with Redis using StackExchange.Redis.
    /// Supports setting, getting, checking, and deleting cached data.
    /// </summary>
    public class RedisService : IRedisService
    {
        #region Fields

        private readonly IDatabase _database;
        private readonly IServer _server;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisService"/> class.
        /// </summary>
        /// <param name="connectionMultiplexer">The Redis connection multiplexer.</param>
        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();

            var endPoint = connectionMultiplexer.GetEndPoints()[0];
            _server = connectionMultiplexer.GetServer(endPoint); // Used for advanced operations like key scanning
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a value in Redis with an optional expiration time.
        /// </summary>
        /// <typeparam name="T">Type of the value to store.</typeparam>
        /// <param name="key">The key under which the value will be stored.</param>
        /// <param name="value">The value to be stored.</param>
        /// <param name="expiration">Optional expiration time.</param>
        /// <returns>True if the operation succeeded; otherwise, false.</returns>
        public async Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            return await _database.StringSetAsync(key, jsonData, expiration);
        }

        /// <summary>
        /// Gets a value from Redis by its key.
        /// </summary>
        /// <typeparam name="T">Expected type of the stored value.</typeparam>
        /// <param name="key">The Redis key.</param>
        /// <returns>The deserialized value or default if not found.</returns>
        public async Task<T?> GetDataAsync<T>(string key)
        {
            var jsonData = await _database.StringGetAsync(key);

            if (jsonData.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        /// <summary>
        /// Checks if a key exists in Redis.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        /// <summary>
        /// Deletes a key from Redis.
        /// </summary>
        /// <param name="key">The key to delete.</param>
        /// <returns>True if the key was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteDataAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Removes all keys from Redis that match a given pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match (e.g., "User_*").</param>
        public async Task RemoveByPatternAsync(string pattern)
        {
            // Note: This uses server-side scan which can be expensive in production with many keys.
            foreach (var key in _server.Keys(pattern: pattern))
            {
                await _database.KeyDeleteAsync(key);
            }
        }

        #endregion
    }
}
