using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor.Lifestyle
{
    public class FluentLifetimeAccessor : IScopeAccessor
    {
        public ILifetimeScope GetScope(CreationContext context)
        {
            return FluentLifetimeScope.GetCurrentLifetimeScope();
        }

        public void Dispose()
        {
            var scope = FluentLifetimeScope.DisposeLifetimeScope();

            scope?.Dispose();
        }
    }
}