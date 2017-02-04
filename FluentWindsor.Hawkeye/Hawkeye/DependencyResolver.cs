using System;

namespace FluentlyWindsor.Hawkeye
{
    public class DependencyResolverException : Exception
    {
        public DependencyResolverException(string message) : base(message)
        {
        }
    }

    public class DependencyResolver
    {
        private static void ThrowIfLoggingWasNotInstalled()
        {
            if (WindsorInstaller.Container == null)
                throw new DependencyResolverException("Please make sure you installed the logging nuget into Windsor using WindsorContainer.Install(new LoggingInstaller()).");
        }

        public static object Resolve(Type type)
        {
            ThrowIfLoggingWasNotInstalled();
            return WindsorInstaller.Container.Resolve(type);
        }

        public static Array ResolveAll(Type type)
        {
            ThrowIfLoggingWasNotInstalled();
            return WindsorInstaller.Container.ResolveAll(type);
        }

        public static T Resolve<T>()
        {
            ThrowIfLoggingWasNotInstalled();
            return WindsorInstaller.Container.Resolve<T>();
        }
        
        public static T[] ResolveAll<T>()
        {
            ThrowIfLoggingWasNotInstalled();
            return WindsorInstaller.Container.ResolveAll<T>();
        }
    }
}