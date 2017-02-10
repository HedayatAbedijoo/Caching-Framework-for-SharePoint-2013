using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Caching
{
    /// <summary>
    /// Class for building cache keys based on method call signatures
    /// </summary>
    public static class CacheKeyBuilder
    {
        /// <summary>
        /// Builds a full cache key using the provided format
        /// </summary>
        /// <remarks>
        /// Returns a hashed GUID of the input, so any size input will be a 16-character key
        /// </remarks>
        /// <param name="keyFormat"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetCacheKey(string keyFormat, params object[] args)
        {
            var cacheKey = keyFormat.FormatWith(args);
            return HashCacheKey(cacheKey);
        }


        /// <summary>
        /// Builds a full cache key in the format:
        ///  ClassName_MethodName_argumentValue1_argumentValue2...._argumentValueN
        /// </summary>
        /// <remarks>
        /// Returns a hashed GUID of the input, so any size input will be a 16-characetr key
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static string GetCacheKey(IMethodInvocation input)
        {
            var prefix = GetCacheKeyPrefix(input) + "_";
            var key = input.ToTraceString(prefix);
            var hashedKey = HashCacheKey(key);
            return hashedKey;
        }

        /// <summary>
        /// Returns the prefix to be used in a full cache key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetCacheKeyPrefix(IMethodInvocation input)
        {
            // Method Name
            return input.MethodBase.Name.ToUpper();
        }

        public static string GetRegion(IMethodInvocation input)
        {
            string id = Microsoft.SharePoint.SPContext.Current.Site.ID.ToString("N") + input.MethodBase.ReflectedType.FullName.ToString().ToLower();
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            id = rgx.Replace(id, "");
            if (!string.IsNullOrEmpty(id))
                return id;
            else
                return string.Empty;
        }
        public static string GetRegion(string interfaceName)
        {
            string id = Microsoft.SharePoint.SPContext.Current.Site.ID.ToString("N") + interfaceName.ToLower();
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            id = rgx.Replace(id, "");
            if (!string.IsNullOrEmpty(id))
                return id;
            else
                return string.Empty;
        }
        public static string GetRegion(string interfaceName, Guid siteId)
        {
            string id = siteId.ToString("N") + interfaceName.ToLower();
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            id = rgx.Replace(id, "");
            if (!string.IsNullOrEmpty(id))
                return id;
            else
                return string.Empty;
        }
        private static string HashCacheKey(string cacheKey)
        {
            //hash the string as a GUID:
            byte[] hashBytes;
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(cacheKey);
                hashBytes = provider.ComputeHash(inputBytes);
            }
            return new Guid(hashBytes).ToString();
        }
    }
}
