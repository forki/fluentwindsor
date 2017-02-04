using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentlyWindsor.Cachely.Interfaces;

namespace FluentlyWindsor.Cachely
{
    public class Cache<T> : IEnumerable<T>, ICache<T>
    {
        private TimeSpan cacheItemExpiryLimit = TimeSpan.MinValue;

        private ThreadSafeDictionary<CacheKey, CacheItem<T>> dictionary =
            new ThreadSafeDictionary<CacheKey, CacheItem<T>>(new Dictionary<CacheKey, CacheItem<T>>());

        private DateTime lastExpired = DateTime.Now;

        public Cache()
        {
            cacheItemExpiryLimit = TimeSpan.FromDays(1);
        }

        public Cache(TimeSpan cacheExpiryLimit)
        {
            cacheItemExpiryLimit = cacheExpiryLimit;
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        public IEnumerable<string> AllKeys
        {
            get
            {
                List<CacheKey> cacheKeys;
                lock (dictionary.SyncRoot)
                    cacheKeys = dictionary.Keys.ToList();
                return cacheKeys.Select(x => (string)x).ToList();
            }
        }

        public virtual bool TryGetValue(string key, out T val)
        {
            AsyncRemoveExpiredItems();

            CacheItem<T> result = default(CacheItem<T>);
            var found = dictionary.TryGetValue(key, out result);
            if (found)
            {
                if (!result.HasExpired)
                {
                    val = result.Value;
                    return true;
                }
                if (result.HasExpired)
                {
                    val = default(T);
                    dictionary.Remove(key);
                    return false;
                }
            }
            val = default(T);
            return false;
        }

        public virtual T GetItem(string key)
        {
            AsyncRemoveExpiredItems();

            CacheItem<T> value;
            var result = dictionary.TryGetValue(key, out value);
            if (result)
            {
                if (!value.HasExpired)
                    return value.Value;
                if (value.HasExpired)
                    dictionary.Remove(key);
            }

            return default(T);
        }

        public virtual T SetItem(string key, object instance)
        {
            AsyncRemoveExpiredItems();

            CacheItem<T> value;
            lock (dictionary)
            {
                if (!dictionary.TryGetValue(key, out value))
                    dictionary.Add(key, new CacheItem<T>(key, (T) instance, cacheItemExpiryLimit));
                else
                {
                    value.Value = (T) instance;
                    value.LastAccessed = DateTime.Now;
                }
            }

            return (T) instance;
        }

        public virtual void ExpireItem(string key)
        {
            var value = default(CacheItem<T>);
            if (dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
            }
        }

        public virtual void Clear()
        {
            lock (dictionary)
                dictionary = new ThreadSafeDictionary<CacheKey, CacheItem<T>>(new Dictionary<CacheKey, CacheItem<T>>());
        }

        public virtual void SetExpiry(TimeSpan cacheItemLifeSpan)
        {
            cacheItemExpiryLimit = cacheItemLifeSpan;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            AsyncRemoveExpiredItems();
            return dictionary.Values.Where(ci => !ci.HasExpired).Select(ci => ci.Value).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void AsyncRemoveExpiredItems()
        {
            if (DateTime.Now.Subtract(lastExpired).TotalSeconds < 5) return;

            lastExpired = DateTime.Now;
            ThreadPool
                .QueueUserWorkItem(
                    s =>
                    {
                        var expiredKeys = dictionary.Values.Where(ci => ci.HasExpired).Select(ci => ci.Key).ToList();
                        expiredKeys.ForEach(key => dictionary.Remove(key));
                    });

            Thread.Sleep(5);
        }
    }
}