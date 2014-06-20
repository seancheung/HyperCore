using HyperCore.Common;
using System.Collections.Generic;
using LitJson;

namespace HyperCore.IO
{
	internal class JsonIO
	{
		/// <summary>
		/// Write a card as Json
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public string Convert(Card card)
		{
			return JsonMapper.ToJson(card);
		}

		/// <summary>
		/// Write a deck as Json
		/// </summary>
		/// <param name="xdeck"></param>
		/// <returns></returns>
		public string Convert(Extern.Hyper.HyperDeck xdeck)
		{
			return JsonMapper.ToJson(xdeck);
		}

		/// <summary>
		/// Read Json as card
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public Card ReadCard(string json)
		{
			return JsonMapper.ToObject<Card>(json);
		}
		/// <summary>
		/// Read Json as deck
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public Extern.Hyper.HyperDeck ReadDeck(string json)
		{
			return JsonMapper.ToObject<Extern.Hyper.HyperDeck>(json);
		}
	}
}
