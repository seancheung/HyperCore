using HyperCore.Common;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HyperCore.IO
{
	[XmlRoot("database")]
	public class Cards
	{
		[XmlArray("cards"),XmlArrayItem("card")]
		public List<Card> cards = new List<Card>();

		/// <summary>
		/// Initializes a new instance of the Cards class with cards
		/// </summary>
		/// <param name="cardList"></param>
		public Cards(IEnumerable<Card> cardList)
		{
			cards = new List<Card>(cardList);
		}

		/// <summary>
		/// Initializes a new instance of the Cards class
		/// </summary>
		public Cards()
		{
		}
	}
}
