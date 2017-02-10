using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class CacheConfiguration : ConfigurationSection
    {
        private static CacheConfiguration _cacheConfig;
        public static CacheConfiguration Current
        {
            get
            {
                if (_cacheConfig != null)
                    return _cacheConfig;
                
                _cacheConfig = ConfigurationManager.GetSection("caching") as CacheConfiguration;
                if (_cacheConfig == null)
                    _cacheConfig = new CacheConfiguration();

                return _cacheConfig;
            }
        }

        /// <summary>
        /// Returns the root path configuration settings
        /// </summary>
        [ConfigurationProperty("enabled", DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

    }
}
