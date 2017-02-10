using Caching;
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace Caching
{
    public class AppFabricCache : ICache
    {
        private DataCache _cache;

        public void Initialise()
        {
            if (_cache == null)
                _cache = SharePointCacheManager.DefaultCache;
        }

        public void Set(string key, object value)
        {
            _cache.Put(key, value);
        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            Set(key, value, new TimeSpan(expiresAt.Subtract(DateTime.Now).Ticks));
        }

        public void Set(string key, object value, TimeSpan validFor)
        {
            _cache.Put(key, value, validFor);
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return Get(key) != null;
        }


        public bool RegionExit(string region)
        {
            return _cache.GetSystemRegions().Contains(region);
        }

        public void Set(string key, string region, object value)
        {
            CheckRegion(region);
            _cache.Put(key, value, region: region);
        }

        public void Set(string key, string region, object value, DateTime expiresAt)
        {
            Set(key, region, value, new TimeSpan(expiresAt.Subtract(DateTime.Now).Ticks));
        }

        public void Set(string key, string region, object value, TimeSpan validFor)
        {
            CheckRegion(region);
            _cache.Put(key, value, validFor, region: region);
        }

        public object Get(string key, string region)
        {
            return _cache.Get(key, region: region);
        }

        public T Get<T>(string key, string region)
        {
            return (T)_cache.Get(key, region: region);

        }

        public bool Exists(string key, string region)
        {
            return Get(key, region) != null;

        }

        private void CheckRegion(string region)
        {
            _cache.CreateRegion(region);
        }


        private void clearRegion(string region)
        {
            try
            {
                _cache.ClearRegion(region);
            }
            catch (Exception)
            {
            }
        }


        public void ClearRegion(string interfaceName)
        {
            if (IsCurrentSiteIsCentralAdministration())
            {
                var ids = getSubWebSiteIds();
                ids.Add(Microsoft.SharePoint.SPContext.Current.Site.ID); // expire Central Admin Zone as well
                foreach (var id in ids)
                {
                    clearRegion(CacheKeyBuilder.GetRegion(interfaceName, id));
                }
            }
            else
            {
                clearRegion(CacheKeyBuilder.GetRegion(interfaceName));
            }
        }

        private bool IsCurrentSiteIsCentralAdministration()
        {
            return Microsoft.SharePoint.SPContext.Current.Site.WebApplication.IsAdministrationWebApplication;
        }

        private IList<Guid> getSubWebSiteIds()
        {
            List<Guid> ids = new List<Guid>();

            SPWebServiceCollection webServices = new SPWebServiceCollection(SPFarm.Local);

            foreach (SPWebService webService in webServices)
            {
                foreach (SPWebApplication webApp in webService.WebApplications)
                {
                    if (!webApp.IsAdministrationWebApplication)
                    {
                        foreach (SPSite item in webApp.Sites)
                        {
                            ids.Add(item.ID);
                        }
                    }
                }
            }

            return ids;
        }
    }
}
