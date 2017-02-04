using System;

namespace FluentlyWindsor.Cachely
{
    public struct CacheItem<T> : IEquatable<CacheItem<T>>
    {
        public static readonly CacheItem<T> Null = new CacheItem<T>(string.Empty, default(T), TimeSpan.MaxValue); 

        public TimeSpan ExpiresIn;
        public string Key;
        public DateTime LastAccessed;
        public T Value;

        public CacheItem(string key, T value, TimeSpan expiresIn)
        {
            Key = key;
            Value = value;
            ExpiresIn = expiresIn;
            LastAccessed = DateTime.Now;
        }

        public bool HasExpired
        {
            get { return DateTime.Now.Subtract(LastAccessed).TotalSeconds > ExpiresIn.TotalSeconds; }
        }

        public bool Equals(CacheItem<T> other)
        {
            return string.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CacheItem<T> && Equals((CacheItem<T>) obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public static bool operator ==(CacheItem<T> left, CacheItem<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CacheItem<T> left, CacheItem<T> right)
        {
            return !left.Equals(right);
        }
    }
}