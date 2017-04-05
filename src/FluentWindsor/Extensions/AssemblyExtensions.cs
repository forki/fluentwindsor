using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentlyWindsor.Interfaces.Policies;

namespace FluentlyWindsor.Extensions
{
    public static class AssemblyExtensions
    {
        static readonly string genericTypeLoadMessage = "FluentWindsor::Assembly Load Errors(If you use nuget please consolidate your versions or try installing the missing assmeblies below, otherwise delete your bin/obj folders outside visual studio and then recompile and fix the missing assembly reference errors in your project) -> \r\n\r\n";

        public static bool HasAnyTypeThatImplementsInterface<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    var interfaces = type.GetInterfaces();
                    if (interfaces.Any(x => x.FullName == typeof(T).FullName))
                        return true;
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return false;
        }

        public static Type[] GetAnyTypeThatImplementsInterface<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            var results = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    var interfaces = type.GetInterfaces();
                    if (interfaces.Any(x => x.FullName == typeof(T).FullName))
                        results.Add(type);
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return results.ToArray();
        }

        public static bool HasAnyTypeThatIsSubClassOf<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    if (type.IsSubclassOf(typeof(T)))
                        return true;
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return false;
        }

        public static Type[] GetAnyTypeThatIsSubClassOf<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            var results = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    if (type.IsSubclassOf(typeof(T)))
                        results.Add(type);
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return results.ToArray();
        }

        public static bool HasAnyTypeThatIs<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    if (type.FullName == typeof(T).FullName)
                        return true;
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return false;
        }

        public static Type[] GetAnyTypeThatIs<T>(this Assembly assembly, IAssemblyScanningPolicy[] policies)
        {
            var results = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    if (type.FullName == typeof(T).FullName)
                        results.Add(type);
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return results.ToArray();
        }

        public static Type[] GetAnyTypeWithFullName(this Assembly assembly, IAssemblyScanningPolicy[] policies, string fullName)
        {
            var results = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes().Where(x => policies.All(y => y.IsTypeAllowed(x))))
                {
                    if (type.FullName.ToLower() == fullName.ToLower())
                        results.Add(type);
                }
            }
            catch (ReflectionTypeLoadException err)
            {
                var loaderErrors = string.Join(",", err.LoaderExceptions.Select(x => x.ToString()));
                throw new Exception(genericTypeLoadMessage + loaderErrors);
            }
            return results.ToArray();
        }
    }
}