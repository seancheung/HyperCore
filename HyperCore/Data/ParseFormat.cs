using HyperCore.Common;
using HyperCore.Exceptions;
using HyperCore.Net;
using HyperCore.Utilities;
using System;
using System.Collections.Generic;

namespace HyperCore.Data
{
	public class ParseFormat
	{
		/// <summary>
		/// Get url of the format data
		/// </summary>
		/// <param name="format">Game format</param>
		/// <returns>An url for webrequesting</returns>
		private static string GetURL(FORMAT format)
		{
			string url;

			switch (format)
			{
				case FORMAT.Standard:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrstandard";
					break;
				case FORMAT.Modern:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrmodern";
					break;
				case FORMAT.Extended:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrextended";
					break;
				case FORMAT.Block:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrblock";
					break;
				case FORMAT.Vintage:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrvintage";
					break;
				case FORMAT.Legacy:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=judge/resources/sfrlegacy";
					break;
				case FORMAT.Classic:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=magic/rules/mtgoclassic";
					break;
				case FORMAT.Commander:
					url = "http://www.wizards.com/Magic/TCG/Resources.aspx?x=magic/rules/100cardsingleton-commander";
					break;
				default:
					url = null;
					break;
			}

			return url;
		}


		private static Format Parse(FORMAT format)
		{
			string webdata;
			try
			{
				webdata = Request.GetWebData(GetURL(format));
			}
			catch
			{
				throw;
			}

			try
			{
				webdata = webdata.Substring(webdata.IndexOf("<div class=\"article-content\">"),
				webdata.IndexOf("</div>", webdata.IndexOf("<div class=\"article-content\">"))
				- webdata.IndexOf("<div class=\"article-content\">"));
				if (!webdata.Contains("<ul>"))
				{
					return null;
				}

				List<string> sets = new List<string>();
				List<string> cards = new List<string>();
				int num = webdata.IndexOf("<ul>");
				num = webdata.IndexOf("<ul>", num);
				int num2 = webdata.IndexOf("</ul>", num);
				string text3 = webdata.Substring(num, num2 - num);
				if (!text3.Contains("keyName=\"name\""))
				{
					string[] array = text3
						.Replace("<ul>", string.Empty)
						.Replace("</ul>", string.Empty)
						.Replace("<i>", string.Empty)
						.Replace("</i>", string.Empty)
						.Replace("<b>", string.Empty)
						.Replace("</b>", string.Empty)
						.Split(new string[]
						{
							"<li>",
							"</li>"
						}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string text4 = array[i];
						if (!string.IsNullOrWhiteSpace(text4))
						{
							sets.Add(text4.RemoveHtmlTag());
						}
					}
					webdata = webdata.Substring(webdata.IndexOf("</ul>", num));
					if (webdata.Contains("keyName=\"name\""))
					{
						int num3 = webdata.IndexOf("<ul>");
						num3 = webdata.IndexOf("<ul>", num3);
						int num4 = webdata.IndexOf("</ul>", num3);
						string text5 = webdata.Substring(num3, num4 - num3);
						array = text5
							.Replace("<ul>", string.Empty)
							.Replace("</ul>", string.Empty)
							.Replace("<i>", string.Empty)
							.Replace("</i>", string.Empty)
							.Replace("<b>", string.Empty)
							.Replace("</b>", string.Empty)
							.Split(new string[]
							{
								"<li>",
								"</li>"
							}, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < array.Length; i++)
						{
							string text4 = array[i];
							if (!string.IsNullOrWhiteSpace(text4))
							{
								cards.Add(text4.Replace("</a>", string.Empty).Substring(text4.IndexOf(">") + 1).RemoveHtmlTag());
							}
						}
					}

				}
				else
				{
					string text5 = text3;
					string[] array = text5
						.Replace("<ul>", string.Empty)
						.Replace("</ul>", string.Empty)
						.Replace("<i>", string.Empty)
						.Replace("</i>", string.Empty)
						.Replace("<b>", string.Empty)
						.Replace("</b>", string.Empty)
						.Split(new string[]
						{
							"<li>",
							"</li>"
						}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string text4 = array[i];
						if (!string.IsNullOrWhiteSpace(text4))
						{
							cards.Add(text4.Replace("</a>", string.Empty).Substring(text4.IndexOf(">") + 1).RemoveHtmlTag());
						}
					}
				}

				return new Format(format, sets, cards);
			}
			catch (Exception ex)
			{
				throw new ParseException("Parsing Error happended when fetching format: " + format.ToString(), ex);
			}
		}

		/// <summary>
		/// Get A list of all Formats
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Format> Parse()
		{
			foreach (FORMAT format in Enum.GetValues(typeof(FORMAT)))
			{
				Format result = null;
				try
				{
					result = Parse(format);
				}
				catch
				{
					throw;
				}
				if (result != null)
				{
					yield return result;
				}
			}
		}
	}
}
