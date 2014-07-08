using HyperCore.Common;
using System.Collections.Generic;

namespace HyperCore.Data
{
	public class ParseCard
	{
		private ParseDetail parseDetailInstance;
		private ParseID parseIDInstance;
		private ParsezID parsezIDInstance;
		private ParsezDetail parsezDetailInstance;
		private ParseLegality parseLegalityInstance;
		private ParseEx parseExInstance;

		/// <summary>
		/// Single Instance
		/// </summary>
		public static readonly ParseCard Instance = new ParseCard();

		/// <summary>
		/// Initializes a new instance of the ParseCard class.
		/// </summary>
		private ParseCard()
		{
			parseDetailInstance = new ParseDetail();
			parsezIDInstance = new ParsezID();
			parsezDetailInstance = new ParsezDetail();
			parseLegalityInstance = new ParseLegality();
			parseExInstance = new ParseEx();
			parseIDInstance = new ParseID();
		}
		/// <summary>
		/// Get a list of cards with ID property filled
		/// </summary>
		/// <param name="setname"></param>
		/// <param name="setcode"></param>
		/// <returns></returns>
		public IEnumerable<Card> GetCards(string setname, string setcode)
		{
			return parseIDInstance.Parse(setname, setcode);
		}

		/// <summary>
		/// Fill card properties
		/// If card is not found, it'll be set NULL
		/// </summary>
		/// <param name="card"></param>
		/// <param name="lang"></param>
		public void Parse(Card card, LANGUAGE lang = LANGUAGE.English)
		{
			try
			{
				parseDetailInstance.Parse(card);
			}
			catch
			{
				throw;
			}

			if (card != null)
			{
				try
				{
					parsezIDInstance.Parse(card, lang);
					parsezDetailInstance.Parse(card);
					parseLegalityInstance.Parse(card);
					parseExInstance.Parse(card);
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
		public Card Parse(string id, LANGUAGE lang = LANGUAGE.English)
		{
			Card card = new Card()
			{
				ID = id
			};
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
