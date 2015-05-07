using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FluentlyWindsor.Extensions;
using FluentlyWindsor.Interfaces;
using FluentlyWindsor.Policies;

namespace FluentlyWindsor
{
    public class FluentWindsor
    {
        private static Assembly executingAssembly;
        private static WindsorContainer container;
        private static ManualResetEvent waitUntilComplete;

        public static Assembly ExecutingAssembly
        {
            get { return executingAssembly; }
        }

        public static IWindsorContainer ServiceLocator
        {
            get { return container; }
        }

        public static ManualResetEvent WaitUntilComplete
        {
            get { return waitUntilComplete; }
        }

        public FluentWindsor(Assembly executingAssembly)
        {
            container = new WindsorContainer();
            waitUntilComplete = new ManualResetEvent(false);
            FluentWindsor.executingAssembly = executingAssembly;
        }

        public static FluentWindsor NewContainer(Assembly executingAssembly)
        {
            return new FluentWindsor(executingAssembly);
        }

        public FluentWindsor WithArrayResolver()
        {
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            return this;
        }

        public FluentWindsor WithInstallers()
        {
            container.Install(new WindsorInstaller());
            var assemblies = container.Resolve<IAssemblyScanner>()
                .FindAssemblies(x => x.HasAnyTypeThatImplementsInterface<IWindsorInstaller>(AssemblyScanningPolicies.All))
                .Concat(new[]{executingAssembly})
                .Distinct();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof (IWindsorInstaller).IsAssignableFrom(type))
                    {
                        var typeInstance = (IWindsorInstaller) Activator.CreateInstance(type);
                        container.Install(typeInstance);
                    }
                }
            }

            return this;
        }

        public FluentWindsor WithTypesInheriting<T>(Action<IWindsorContainer, Type> registrationAction)
        {
            var types = container.Resolve<IAssemblyScanner>()
                .FindAssemblies(x => x.HasAnyTypeThatIsSubClassOf<T>(AssemblyScanningPolicies.All))
                .SelectMany(x => x.GetAnyTypeThatIsSubClassOf<T>(AssemblyScanningPolicies.All))
                .Concat(executingAssembly.GetAnyTypeThatIsSubClassOf<T>(AssemblyScanningPolicies.All));

            foreach(var type in types)
                if (registrationAction != null)
                    registrationAction(container, type);

            return this;
        }

        public IWindsorContainer Create()
        {
            waitUntilComplete.Set();
            return container;
        }
    }
}