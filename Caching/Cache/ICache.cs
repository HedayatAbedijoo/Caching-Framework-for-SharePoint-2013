using System;
using System.Collections.Generic;

namespace Caching
{
    public interface ICache
    {

        /// <summary>
        /// Performs initialisation tasks required for the cache implementation
        /// </summary>
        void Initialise();

        /// <summary>
        /// Insert or update a cache value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value);

        /// <summary>
        /// Insert or update a cache value with an expiry date
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value, DateTime expiresAt);

        /// <summary>
        /// Insert or update a cache value with a fixed lifetime
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="validFor"></param>
        /// <param name="serializationFormat"></param>
        void Set(string key, object value, TimeSpan validFor);

        /// <summary>
        /// Retrieve a value from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serializationFormat"></param>
        /// <returns>Cached value or null</returns>
        object Get(string key);

        /// <summary>
        /// Retrieve a typed value from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="serializationFormat"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Removes the value for the given key from the cache
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Returns whether the cache contains a value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        bool RegionExit(string region);
        void Set(string key, string region, object value);
        void Set(string key, string region, object value, DateTime expiresAt);
        void Set(string key, string region, object value, TimeSpan validFor);
        object Get(string key, string region);
        T Get<T>(string key, string region);
        bool Exists(string key, string region);
        void ClearRegion(string interfaceName);
    }
}
