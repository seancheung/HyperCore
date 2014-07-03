using System.Collections.Generic;
using System.Xml;

namespace HyperCore.IO
{
	public class AppData
	{
		public static readonly AppData Instance = new AppData();

		public class Data
		{
			public string Name { get; set; }
			public string Code { get; set; }
		}

		public IEnumerable<Data> Load(string key, string xmlPath)
		{
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(xmlPath);

			var nodelist = xdoc.SelectSingleNode("/appdata/" + key);
			if (nodelist == null || !nodelist.HasChildNodes)
			{
				yield break;
			}

			foreach (XmlNode node in nodelist)
			{
				yield return new Data() { Name = node.Attributes["name"].Value, Code = node.Attributes["code"].Value };
			}
			
		}
	}
}
