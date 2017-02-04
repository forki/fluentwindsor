using System;
using System.Collections.Generic;

namespace FluentlyWindsor.Cachely.Interfaces
{
    public interface ICache<T>
    {
        IEnumerator<T> GetEnumerator();
        bool TryGetValue(string key, out T val);
        T GetItem(string key);
        T SetItem(string key, object instance);
        void ExpireItem(string key);
        void Clear();
        void SetExpiry(TimeSpan cacheItemLifeSpan);
        IEnumerable<string> AllKeys { get; }
    }
}