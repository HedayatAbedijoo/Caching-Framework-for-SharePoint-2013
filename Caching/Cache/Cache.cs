
using Caching;
using System;
using System.Linq;

namespace Caching
{
    /// <summary>
    /// Wrapper for accessing <see cref="ICache"/> implementations
    /// </summary>
    public static class Cache
    {
        public static ICache Get()
        {
            ICache cache;
            try
            {
                cache = new AppFabricCache();                
                cache.Initialise();
            }
            catch (Exception ex)
            {
                return null; 
            }
            return cache;
        }

      
        public static ICache AppFabric
        {
            get
            {
                return Get();
            }
        }
      
    }
}
