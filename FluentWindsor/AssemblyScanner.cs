using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentlyWindsor.Interfaces;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly IAssemblyScanningPolicy[] policies;

        public AssemblyScanner(IAssemblyScanningPolicy[] policies)
        {
            this.policies = policies;
        }

        public virtual List<Assembly> FindAssemblies(Predicate<Assembly> isTrueOf)
        {
            //Debugger.Launch();

            var results = new List<Assembly>();
            var applicationDomain = new ApplicationDomain(AppDomain.CurrentDomain);

            foreach (var fileInfo in applicationDomain.GetAssemblyFiles())
                applicationDomain.Load(fileInfo.Name.Replace(fileInfo.Extension, ""));

            foreach (var assembly in applicationDomain.GetAssemblies().Where(x => policies.All(y => y.IsAssemblyAllowed(x))))
            {
                if (isTrueOf(assembly))
                {
                    results.Add(assembly);
                }
            }

            return results;
        }
    }
}