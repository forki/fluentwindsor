using System;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Build.Framework;

namespace BuildTasks
{
    public class MigratePackageDependenciesToNuSpec : ITask
    {
        [Required]
        public string RelativePath { get; set; }

        public bool Execute()
        {
            var currentDirectory = Path.Combine(Environment.CurrentDirectory, RelativePath);

            var files = Directory.GetFiles(currentDirectory, "nuget.nuspec", SearchOption.AllDirectories);

            foreach (var nuspecFile in files)
            {
                Console.WriteLine($"Inspecting: {nuspecFile}");

                var nuspecXmlFile = new XmlDocument();

                nuspecXmlFile.Load(nuspecFile);

                var dependencyNodes = nuspecXmlFile.SelectNodes("/package/metadata/dependencies/dependency").Cast<XmlNode>().ToList();

                foreach (var dependencyNode in dependencyNodes)
                {
                    var packageId = dependencyNode.Attributes["id"].Value;

                    var expectedPackageVersion = dependencyNode.Attributes["version"].Value;

                    Console.WriteLine($"Found dependency: {packageId}, {expectedPackageVersion}");

                    var packageDirectory = Path.Combine(currentDirectory, "packages");

                    Console.WriteLine($"Searching packages: {packageDirectory}");

                    foreach (var packageFolder in Directory.EnumerateDirectories(packageDirectory))
                    {
                        if (packageFolder.ToLower().EndsWith(packageId.ToLower()))
                        {
                            Console.WriteLine($"Found package: {packageFolder}");

                            var nuspecPackageFiles = Directory.GetFiles(packageFolder, "*.nuspec", SearchOption.TopDirectoryOnly);

                            foreach (var targetNuspecPackageFile in nuspecPackageFiles)
                            {
                                var targetNuspecXmlFile = new XmlDocument();

                                targetNuspecXmlFile.Load(targetNuspecPackageFile);

                                var targetNuspecVersionNode = targetNuspecXmlFile.SelectSingleNode("/*[local-name()='package']/*[local-name()='metadata']/*[local-name()='version']");

                                var actualPackageVersion = targetNuspecVersionNode.InnerText;

                                Console.WriteLine($"Found version: Expected {expectedPackageVersion} -> Actual {actualPackageVersion}");

                                if (expectedPackageVersion != actualPackageVersion)
                                {
                                    Console.WriteLine($"Upgrading version: {actualPackageVersion}");

                                    dependencyNode.Attributes["version"].Value = actualPackageVersion;

                                    nuspecXmlFile.Save(nuspecFile);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public IBuildEngine BuildEngine { get; set; }

        public ITaskHost HostObject { get; set; }
    }
}