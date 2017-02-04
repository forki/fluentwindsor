using System;

namespace FluentlyWindsor.Cachely
{
    public struct CacheKey : IEquatable<CacheKey>
    {
        public readonly int Hash;
        public readonly string Key;

        public CacheKey(string key, int hash)
        {
            Key = key;
            Hash = hash;
        }

        public static implicit operator string(CacheKey d)
        {
            return d.Key;
        }

        public static implicit operator CacheKey(string d)
        {
            return new CacheKey(d, d.GetHashCode());
        }

        public override int GetHashCode()
        {
            return Hash;
        }

        public bool Equals(CacheKey other)
        {
            return Hash == other.Hash;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CacheKey && Equals((CacheKey) obj);
        }

        public static bool operator ==(CacheKey left, CacheKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CacheKey left, CacheKey right)
        {
            return !left.Equals(right);
        }
    }
}