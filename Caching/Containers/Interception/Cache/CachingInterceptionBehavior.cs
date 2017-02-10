using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Containers.Interception.Cache
{
    class CachingInterceptionBehavior : IInterceptionBehavior
    {
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            #region Check cache is enabled
            if (!CacheConfiguration.Current.Enabled)
            {
                return Proceed(input, getNext);
            }
            #endregion

            #region NoCache is checking
            var nocache = input.MethodBase.GetCustomAttributes(typeof(NoCacheAttribute), false);
            if (nocache.Count() != 0)
            {
                return Proceed(input, getNext);
            }
            #endregion

            #region Instanciate AppFabric Cache. if it is no working, return.
            var cache = Caching.Cache.AppFabric;
            if (cache == null /* or cache is disabled inside the webconfig*/)
            {
                return Proceed(input, getNext);
            }
            #endregion

            ExpireRegions(input, cache);

            #region Check Void Methods
            if (((MethodInfo)input.MethodBase).ReturnType.Name == "Void") // specific behavior for Void Methods
            {
                return Proceed(input, getNext);
            }


            #endregion

            #region Check the Cache
            string cacheKey = CacheKeyBuilder.GetCacheKey(input);
            string region = CacheKeyBuilder.GetRegion(input);
            var cachedValue = cache.Get(cacheKey, region);

            if (cachedValue == null)
            {
                var methodReturn = Proceed(input, getNext);
                if (methodReturn != null && methodReturn.ReturnValue != null && methodReturn.Exception == null)
                {
                    cache.Set(cacheKey, region, methodReturn.ReturnValue);
                }
                return methodReturn;
            }
            else
            {
                return input.CreateMethodReturn(cachedValue);
            }
            #endregion

        }

        private IMethodReturn Proceed(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            return getNext()(input, getNext);
        }

        public bool WillExecute
        {
            get { return true; }
        }


        private void ExpireRegions(IMethodInvocation input, ICache cache)
        {
            var exipreAttributes = input.MethodBase.GetCustomAttributes(typeof(ExpireCacheAttribute), false);
            if (exipreAttributes.Count() == 0) return;

            var expAttr = (ExpireCacheAttribute)exipreAttributes[0];
            foreach (var item in expAttr.Regions)
            {
                cache.ClearRegion(item.FullName);
            }
        }

    }
}
