using System.Diagnostics;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;

namespace FluentlyWindsor.Mvc
{
    public class PerWebRequestLifestyleManager : AbstractLifestyleManager
    {
        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            Debug.WriteLine($"PerWebRequestLifestyleManager: Resolve: {context.RequestedType.FullName}");

            return base.Resolve(context, releasePolicy);
        }

        public override bool Release(object instance)
        {
            var wasReleased = base.Release(instance);

            Debug.WriteLine($"PerWebRequestLifestyleManager: Release: {instance.GetType().FullName}, wasRelease={wasReleased}");

            return wasReleased;
        }

        public override void Dispose()
        {
            Debug.WriteLine("PerWebRequestLifestyleManager: Dispose called ...");
        }
    }
}