using HyperCore.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperCore.Data
{
	public class ParseDefault
	{
		public IEnumerable<string> ParseFormat()
		{
			return Parse("Card Format:").ToArray();

		}

		public IEnumerable<string> ParseSet()
		{
			return Parse("Card Set:").ToArray();

		}

		public IEnumerable<string> ParseType()
		{
			return Parse("Card Type:").ToArray();

		}

		private IEnumerable<string> Parse(string header)
		{
			string webdata;
			try
			{
				webdata = Request.Instance.GetWebData(GetURL());
			}
			catch
			{
				throw;
			}

			if (!webdata.Contains(header))
			{
				yield break;
			}

			var startidx = webdata.IndexOf(header);
			var endidx = webdata.IndexOf("</select>", startidx);
			webdata = webdata.Substring(webdata.IndexOf("<select", startidx), endidx - startidx);

			var idxa = webdata.IndexOf("<option");
			while (idxa > 0)
			{
				idxa = webdata.IndexOf(">", idxa) + 1;
				var idxb = webdata.IndexOf("</option>", idxa);
				var content = webdata.Substring(idxa, idxb - idxa);
				if (!string.IsNullOrWhiteSpace(content))
				{
					yield return content.Trim();
				}

				idxa = webdata.IndexOf("<option", idxb);
			}
		}

		/// <summary>
		/// Get url of the default page
		/// </summary>
		/// <returns>the url for webrequesting</returns>
		private string GetURL()
		{
			return @"http://gatherer.wizards.com/Pages/Default.aspx";
		}
	}
}
