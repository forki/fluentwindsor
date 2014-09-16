using System;
using System.Linq;
using System.Xml;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(@"D:\Gavin\Code\FluentWindsor\FluentWindsor\nuget.nuspec");

            var node = xmlDocument.SelectNodes("/package/metadata/version").Cast<XmlNode>().FirstOrDefault();

            if (node == null)
                throw new Exception("Think your xpath is wrong ... ");

            var nodeContent = node.InnerText;

            var versionNumbers = nodeContent.Split('.');

            var bumpVersion = int.Parse(versionNumbers.Last()) + 1;

            versionNumbers[versionNumbers.Length - 1] = bumpVersion.ToString();

            node.InnerText = string.Join(".", versionNumbers);

            xmlDocument.Save(@"D:\Gavin\Code\FluentWindsor\FluentWindsor\nuget.nuspec");
        }
    }
}
