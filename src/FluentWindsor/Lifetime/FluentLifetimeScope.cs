using System;
using System.Collections.Concurrent;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor.Lifestyle
{
    public class FluentLifetimeScope : ILifetimeScope
    {
        private static readonly ConcurrentDictionary<Guid, FluentLifetimeScope> localScopeCache = new ConcurrentDictionary<Guid, FluentLifetimeScope>();

        private readonly Guid contextId;

        private readonly Lock @lock = Lock.Create();

        private ScopeCache cachedInstances = new ScopeCache();

        public static Func<ILifetimeScope> DisposeLifetimeScope;

        public static Func<ILifetimeScope> GetCurrentLifetimeScope;

        public Guid ContextId => contextId;

        public FluentLifetimeScope()
        {
            contextId = Guid.NewGuid();

            localScopeCache.TryAdd(contextId, this);
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

            FluentLifetimeScope @this;

            localScopeCache.TryRemove(contextId, out @this);
        }
    }
}