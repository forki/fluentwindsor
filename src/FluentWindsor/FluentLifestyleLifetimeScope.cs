using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
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


        static AsyncLocal<FluentLifestyleLifetimeScope> _asyncLocalString = new AsyncLocal<FluentLifestyleLifetimeScope>();

        private static readonly ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope> localScopeCache = new ConcurrentDictionary<Guid, FluentLifestyleLifetimeScope>();

        private readonly Guid contextId;

        private readonly Lock @lock = Lock.Create();

        private ScopeCache cache = new ScopeCache();

        public static Func<ILifetimeScope> DisposeLifetimeScope;

        public static Func<ILifetimeScope> GetCurrentLifetimeScope;

        private readonly FluentLifestyleLifetimeScope parentScope;

        public FluentLifestyleLifetimeScope()
        {
            var parent = GetCurrentLifetimeScope();

            if (parent != null)
            {
                parentScope = (FluentLifestyleLifetimeScope) parent;
            }

            contextId = Guid.NewGuid();

            var added = localScopeCache.TryAdd(contextId, this);

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

            localScopeCache.TryRemove(contextId, out @this);
        }
    }
}