using System;
using System.Linq;
using System.Xml;
using Microsoft.Build.Framework;

namespace BuildTasks
{
	public class NuspecSetVersion : ITask
	{
		[Required]
		public string FilePath { get; set; }

		[Required]
		public string XPathToVersionNumber { get; set; }

		public bool Execute()
		{
			var xmlDocument = new XmlDocument();

			xmlDocument.Load(FilePath);

			var node = xmlDocument.SelectNodes(XPathToVersionNumber).Cast<XmlNode>().FirstOrDefault();

			if (node == null)
				throw new Exception("Think your xpath is wrong ... ");

			node.InnerText = string.Join(".", Environment.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION"));

			xmlDocument.Save(FilePath);

			return true;
		}

		public IBuildEngine BuildEngine { get; set; }

		public ITaskHost HostObject { get; set; }
	}
}