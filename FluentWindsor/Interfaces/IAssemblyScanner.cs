using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentlyWindsor.Interfaces
{
    public interface IAssemblyScanner
    {
        List<Assembly> FindAssemblies(Predicate<Assembly> isTrueOf);
    }
}