using System;
using System.Collections.Concurrent;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor.Lifestyle
{
    public class FluentLifestyleLifetimeScope : ILifetimeScope
    {
        private static readonly ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope> localScopeCache = new ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope>();

        private readonly Guid contextId;

        private readonly Lock @lock = Lock.Create();

        private ScopeCache cachedInstances = new ScopeCache();

        public static Func<ILifetimeScope> DisposeLifetimeScope;

        public static Func<ILifetimeScope> GetCurrentLifetimeScope;

        public Guid ContextId => contextId;

        public FluentLifestyleLifetimeScope()
        {
            contextId = Guid.NewGuid();

            var added = localScopeCache.TryAdd(contextId, this);
        }

        public Burden GetCachedInstance(ComponentModel model, ScopedInstanceActivationCallback createInstance)
        {
            using (var token = @lock.ForReadingUpgradeable())
            {
                var burden = cachedInstances[model];

                if (burden == null)
                {
                    token.Upgrade();

                    burden = createInstance(delegate { });

                    cachedInstances[model] = burden;
                }

                return burden;
            }
        }

        public void Dispose()
        {
            using (var token = @lock.ForReadingUpgradeable())
            {
                if (cachedInstances == null)
                {
                    return;
                }

                token.Upgrade();

                cachedInstances.Dispose();

                cachedInstances = null;
            }

            FluentLifestyleLifetimeScope @this;

            var removed = localScopeCache.TryRemove(contextId, out @this);
        }
    }
}