using HyperCore.Common;
using System.Collections.Generic;

namespace HyperCore.Data
{
	public class ParseCard
	{

		/// <summary>
		/// Get a list of cards with ID property filled
		/// </summary>
		/// <param name="setname"></param>
		/// <param name="setcode"></param>
		/// <returns></returns>
		public static IEnumerable<Card> GetCards(string setname, string setcode)
		{
			return ParseID.Parse(setname, setcode);
		}

		/// <summary>
		/// Fill card properties
		/// </summary>
		/// <param name="card"></param>
		/// <param name="lang"></param>
		public static void Parse(Card card, LANGUAGE lang = LANGUAGE.English)
		{
			try
			{
				ParseDetail.Parse(card);
			}
			catch
			{
				throw;
			}

			if (card != null)
			{
				try
				{
					ParsezID.Parse(card, lang);
					ParsezDetail.Parse(card);
					ParseLegality.Parse(card);
					ParseEx.Parse(card);
				}
				catch
				{
					throw;
				}
			}

		}

		/// <summary>
		/// Get a card by ID and fill all its properties
		/// </summary>
		/// <param name="id"></param>
		/// <param name="lang"></param>
		/// <returns></returns>
		public static Card Parse(string id, LANGUAGE lang = LANGUAGE.English)
		{
			Card card = new Card() { ID = id };
			try
			{
				Parse(card, lang);
			}
			catch
			{
				throw;
			}

			return card;
		}
	}
}
