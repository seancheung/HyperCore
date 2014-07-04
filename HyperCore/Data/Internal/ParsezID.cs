using HyperCore.Common;
using HyperCore.Exceptions;
using HyperCore.Net;
using System;

namespace HyperCore.Data
{
	internal class ParsezID
	{

		private void GetzID(Card card,LANGUAGE lang)
		{
			string webdata;
			try
			{
				webdata = Request.Instance.GetWebData(GetURL(card.ID));
			}
			catch
			{
				throw;
			}

			if (lang == LANGUAGE.English || !webdata.Contains("This card is available in the following languages:") || !webdata.Contains(lang.ToString().Replace("_", " ")))
			{
				card.zID = string.Empty;
			}
			else
			{
				try
				{
					webdata = webdata.Remove(webdata.IndexOf(lang.ToString().Replace("_", " ")));
					int num = webdata.LastIndexOf("multiverseid=") + 13;
					int num2 = webdata.IndexOf("\"", num);
					card.zID = webdata.Substring(num, num2 - num);
				}
				catch (Exception ex)
				{
					throw new ParseException(card, "Parsing Error happended when parsing card zID:" + lang.ToString(), ex);
				}
			}
		}

		/// <summary>
		/// Get url of the card list data
		/// </summary>
		/// <param name="id">Card ID</param>
		/// <returns>the url for webrequesting</returns>
		private string GetURL(string id)
		{
			if (id.Contains("|"))
			{
				id = id.Remove(id.IndexOf("|"));
			}

			return string.Format("http://gatherer.wizards.com/Pages/Card/Languages.aspx?multiverseid={0}", id);
		}

		/// <summary>
		/// Fill card foreign ID
		/// </summary>
		/// <param name="card">Card to process</param>
		/// <param name="lang">Language</param>
		public void Parse(Card card, LANGUAGE lang)
		{
			GetzID(card, lang);

			//use traditional chinese in case of simplified being unavailable
			if (card.zID == string.Empty && lang == LANGUAGE.Chinese_Simplified)
			{
				GetzID(card, LANGUAGE.Chinese_Traditional);
			}

		}
	}
}
