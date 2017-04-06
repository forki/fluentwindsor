using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor
{
    public class FluentLifestyleLifetimeScope : ILifetimeScope
    {
        public FluentLifestyleLifetimeScope CurrentScope = null;

        private static readonly ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope> appDomainLocalInstanceCache = new ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope>();

        private readonly Guid contextId;

        private readonly Lock @lock = Lock.Create();

        private ScopeCache cache = new ScopeCache();

        public static Func<ILifetimeScope> DisposeLifetimeScope;

        public static Func<CreationContext, ILifetimeScope> GetCurrentLifetimeScope;

        private readonly FluentLifestyleLifetimeScope parentScope;

        public FluentLifestyleLifetimeScope()
        {
            var parent = GetCurrentLifetimeScope(null);

            if (parent != null)
            {
                parentScope = (FluentLifestyleLifetimeScope) parent;
            }
            contextId = Guid.NewGuid();

            var added = appDomainLocalInstanceCache.TryAdd(contextId, this);

            Debug.Assert(added);

            CurrentScope = this;
        }

        public Burden GetCachedInstance(ComponentModel model, ScopedInstanceActivationCallback createInstance)
        {
            using (var token = @lock.ForReadingUpgradeable())
            {
                var burden = cache[model];

                if (burden == null)
                {
                    token.Upgrade();

                    burden = createInstance(delegate { });

                    cache[model] = burden;
                }

                return burden;
            }
        }

        public void Dispose()
        {
            using (var token = @lock.ForReadingUpgradeable())
            {
                if (cache == null)
                {
                    return;
                }

                token.Upgrade();

                cache.Dispose();

                cache = null;

                if (parentScope != null)
                {
                    CurrentScope = parentScope;
                }
                else
                {
                    CurrentScope.Dispose();
                }
            }
            FluentLifestyleLifetimeScope @this;
            appDomainLocalInstanceCache.TryRemove(contextId, out @this);
        }
    }
}