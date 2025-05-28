namespace RestrantApplication.Core.Services
{
    public interface IRedisService
    {
        /// <summary>
        /// Stores a value in Redis under the specified key, with optional expiration.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="key">The key to associate with the stored value.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="expiration">Optional expiration time for the key.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        Task<bool> SetDataAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Retrieves a value from Redis using the specified key.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored value.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <returns>The value if found; otherwise, null.</returns>
        Task<T?> GetDataAsync<T>(string key);

        /// <summary>
        /// Checks if a key exists in Redis.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        Task<bool> KeyExistsAsync(string key);

        /// <summary>
        /// Deletes a value from Redis using the specified key.
        /// </summary>
        /// <param name="key">The key of the value to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteDataAsync(string key);

        /// <summary>
        /// Removes all keys that match the given pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match keys against.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveByPatternAsync(string pattern);
    }
}
