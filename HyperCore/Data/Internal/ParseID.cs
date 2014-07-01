using HyperCore.Common;
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
				webdata = Request.GetWebData(GetURL_checklist(setname));
			}
			catch
			{
				throw;
			}

			if (webdata.Contains("Your search returned zero results"))
			{
				yield break;
			}

			while (webdata.Contains("multiverseid="))
			{
				Card card = new Card();
				try
				{
					var numa = webdata.IndexOf("\"number\"") + 9;
					var numb = webdata.IndexOf("<", numa);
					card.Number = webdata.Substring(numa, numb - numa);

					var ida = webdata.IndexOf("multiverseid=") + 13;
					var idb = webdata.IndexOf("\"", ida);
					card.ID = webdata.Substring(ida, idb - ida);

					var namea = webdata.IndexOf(">", idb) + 1;
					var nameb = webdata.IndexOf("<", namea);
					card.Name = webdata.Substring(namea, nameb - namea);

					var arta = webdata.IndexOf("\"artist\"") + 9;
					var artb = webdata.IndexOf("<", arta);
					card.Artist = webdata.Substring(arta, artb - arta);

					var coa = webdata.IndexOf("\"color\"") + 8;
					var cob = webdata.IndexOf("<", coa);
					card.Color = webdata.Substring(coa, cob - coa).Replace("/", " ");

					var rca = webdata.IndexOf("\"rarity\"") + 9;
					var rcb = webdata.IndexOf("<", rca);
					card.RarityCode = webdata.Substring(rca, rcb - rca).Replace("/", " ");

					card.Set = setname;
					card.SetCode = setcode;

					webdata = webdata.Substring(idb);

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
		private static string GetURL_compact(string setname)
		{
			return string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=compact&set=%5b%22{0}%22%5d", setname.Replace(" ", "+"));
		}

		/// <summary>
		/// Get url of the card list data
		/// </summary>
		/// <param name="setname">Full english set name</param>
		/// <param name="setcode">Setcode in capital</param>
		/// <returns>the url for webrequesting</returns>
		private static string GetURL_standard(string setname)
		{
			return string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=standard&set=%5b%22{0}%22%5d", setname.Replace(" ", "+"));
		}

		/// <summary>
		/// Get url of the card list data
		/// </summary>
		/// <param name="setname">Full english set name</param>
		/// <param name="setcode">Setcode in capital</param>
		/// <returns>the url for webrequesting</returns>
		private static string GetURL_checklist(string setname)
		{
			return string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=checklist&set=%5b%22{0}%22%5d", setname.Replace(" ", "+"));
		}

		/// <summary>
		/// Get url of the card list data
		/// </summary>
		/// <param name="setname">Full english set name</param>
		/// <param name="setcode">Setcode in capital</param>
		/// <returns>the url for webrequesting</returns>
		private static string GetURL_visual(string setname)
		{
			return string.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=spoiler&method=visual&set=%5b%22{0}%22%5d", setname.Replace(" ", "+"));
		}
	}
}
