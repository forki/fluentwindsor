using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace FluentlyWindsor
{
    public class FluentLifestyleScopeAccessor : IScopeAccessor
    {
        public ILifetimeScope GetScope(CreationContext context)
        {
            return FluentLifestyleLifetimeScope.GetCurrentLifetimeScope(context);
        }

        public void Dispose()
        {
            var scope = FluentLifestyleLifetimeScope.DisposeLifetimeScope();

            scope?.Dispose();
        }
    }
}