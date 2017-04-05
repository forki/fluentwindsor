using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FluentlyWindsor
{
    public class ApplicationDomain
    {
        private readonly AppDomain appDomain;

        public ApplicationDomain(AppDomain appDomain)
        {
            this.appDomain = appDomain;
        }

        public FileInfo[] GetAssemblyFiles()
        {
            var directoryInfo = new DirectoryInfo(appDomain.BaseDirectory);

            if (Directory.Exists(appDomain.BaseDirectory + "\\bin"))
                directoryInfo = new DirectoryInfo(appDomain.BaseDirectory + "\\bin");

            var files = directoryInfo.GetFiles().Where(x => x.FullName.ToLower().EndsWith(".dll") || x.FullName.ToLower().EndsWith(".exe")).ToArray();
            return files;
        }

        public Assembly[] GetAssemblies()
        {
            return appDomain.GetAssemblies();
        }

        [DebuggerStepThrough]
        public void Load(string assemblyName)
        {
            try
            {
                appDomain.Load(assemblyName);
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            AppDomain.Unload(appDomain);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
        }
    }
}