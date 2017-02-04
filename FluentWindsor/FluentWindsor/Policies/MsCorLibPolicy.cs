using System;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Policies
{
    public class MsCorLibPolicy : IAssemblyScanningPolicy
    {
        public bool IsAssemblyAllowed(Assembly assembly)
        {
            return !assembly.FullName.StartsWith("mscorlib");
        }
        
        public bool IsTypeAllowed(Type type)
        {
            return true;
        }
    }
}