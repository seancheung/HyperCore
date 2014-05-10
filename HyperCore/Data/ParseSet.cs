using HyperCore.Exceptions;
using HyperCore.Net;
using System;
using System.Collections.Generic;

namespace HyperCore.Data
{
	public class ParseSet
	{
		/// <summary>
		/// Get a list of all available sets
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> Parse()
		{
			string webdata;
			try
			{
				webdata = Request.GetWebData(GetURL());
			}
			catch
			{
				throw;
			}

			if (!webdata.Contains("<label for=\"edition\">Edition:</label>"))
			{
				yield break;
			}
			else
			{
				try
				{
					webdata = webdata.Substring(webdata.IndexOf("<label for=\"edition\">Edition:</label>"), webdata.IndexOf("<i>Use SHIFT and CTRL to select more than one edition.</i>") - webdata.IndexOf("<label for=\"edition\">Edition:</label>"));
					webdata = webdata.Substring(webdata.IndexOf("<optgroup"));
				}
				catch (Exception ex)
				{
					throw new ParseException("Parsing Error happended when fetching Set list - step.1", ex);
				}

				while (webdata.Contains("value="))
				{
					string set = string.Empty;
					try
					{
						int num = webdata.IndexOf("/en\">") + 5;
						int num2 = webdata.IndexOf("</option>", num);
						int num3 = webdata.IndexOf("<option value=") + 15;
						int num4 = webdata.IndexOf("/en\">", num3);
						set = String.Format("{0}({1})", webdata.Substring(num, num2 - num), webdata.Substring(num3, num4 - num3).ToUpper());
						webdata = webdata.Substring(num2);
					}
					catch (Exception ex)
					{
						throw new ParseException("Parsing Error happended when fetching Set list - step.2", ex);
					}
					if (!string.IsNullOrWhiteSpace(set))
					{
						yield return set;
					}
				}
			}
		}

		/// <summary>
		/// Get the set list url
		/// </summary>
		/// <returns>the url for webrequesting</returns>
		private static string GetURL()
		{
			return string.Format(@"http://magiccards.info/search.html");
		}
	}
}
