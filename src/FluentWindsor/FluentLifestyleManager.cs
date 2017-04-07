using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor
{
    public class FluentLifestyleManager : AbstractLifestyleManager
    {
        private IScopeAccessor accessor;

        public FluentLifestyleManager()
            : this(new FluentLifestyleScopeAccessor())
        {
        }

        public FluentLifestyleManager(IScopeAccessor accessor)
        {
            this.accessor = accessor;
        }

        public override void Dispose()
        {
            var scope = Interlocked.Exchange(ref accessor, null);

            scope?.Dispose();
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var scope = GetScope(context);

            var burden = scope.GetCachedInstance(Model, afterCreated =>
            {
                var localBurden = base.CreateInstance(context, trackedExternally: true);

                afterCreated(localBurden);

                Track(localBurden, releasePolicy);

                localBurden.GraphReleased += burden1 =>
                {
                    Debug.WriteLine("Burden: Graph Released");
                };

                localBurden.Released += burden1 =>
                {
                    Debug.WriteLine("Burden: Released");
                };

                localBurden.Releasing += burden1 =>
                {
                    Debug.WriteLine("Burden: Releasing");
                };

                return localBurden;
            });

            return burden.Instance;
        }

        private ILifetimeScope GetScope(CreationContext context)
        {
            var localScope = accessor;

            if (localScope == null)
            {
                throw new ObjectDisposedException("Scope was already disposed. This is most likely a bug in the calling code.");
            }

            var scope = localScope.GetScope(context);

            if (scope == null)
            {
                throw new ComponentResolutionException($"Could not obtain scope for component {Model.Name}. This is most likely either a bug in custom {typeof(IScopeAccessor).ToCSharpString()} or you're trying to access scoped component outside of the scope (like a per-web-request component outside of web request etc)", Model);
            }

            return scope;
        }
    }
}