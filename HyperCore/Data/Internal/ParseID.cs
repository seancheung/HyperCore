using HyperCore.Exceptions;
using HyperCore.Net;
using System;
using System.Collections.Generic;

namespace HyperCore.Data
{
	internal class ParseID
	{
		private ParseID() { }

		/// <summary>
		/// Get a list of cards with ID property filled
		/// </summary>
		/// <param name="setname">Full english set name</param>
		/// <param name="setcode">Setcode in capital</param>
		/// <returns>A list of cards</returns>
		public static IEnumerable<Card> Parse(string setname, string setcode)
		{
			string webdata;
			try
			{
				webdata = Request.GetWebData(GetURL(setname, setcode));
			}
			catch
			{
				throw;
			}

			if (webdata.Contains("Your search returned zero results"))
			{
				yield break;
			}

			for (int i = webdata.IndexOf("href=\"../Card/Details.aspx?multiverseid="); i > 0; i = webdata.IndexOf("href=\"../Card/Details.aspx?multiverseid="))
			{
				Card card = new Card();

				try
				{
					webdata = webdata.Remove(0, i + 40);
					int length = webdata.IndexOf("\">");
					int coststart = webdata.IndexOf("<td>", webdata.IndexOf("Cost:")) + 4;
					int costend = webdata.IndexOf("</td>", coststart);
					int textstart = webdata.IndexOf("<td>", webdata.IndexOf("Rules Text:")) + 4;
					int textend = webdata.IndexOf("</td>", textstart);

					card.ID = webdata.Substring(0, length);
					card.Cost = webdata.Substring(coststart, costend - coststart).Trim();
					card.Text = webdata.Substring(textstart, textend - textstart).Replace("<br />", "\n").Trim();
					card.Set = setname;
					card.SetCode = setcode;
				}
				catch (Exception ex)
				{
					throw new ParseException(card, "Parsing Error happended when fetching ID list", ex);
				}
				yield return card;
			}

		}

		/// <summary>
		/// Get url of the card list data
		/// </summary>
		/// <param name="setname">Full english set name</param>
		/// <param name="setcode">Setcode in capital</param>
		/// <returns>the url for webrequesting</returns>
		private static string GetURL(string setname, string setcode)
		{
			return string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=spoiler&method=text&action=advanced&set=+%5b%22{0}%22%5d", setname.Replace(" ", "+"));
		}
	}
}
