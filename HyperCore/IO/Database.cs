using HyperCore.Common;
using HyperCore.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;

namespace HyperCore.IO
{
	public class Database
	{
		/// <summary>
		/// Class for Serialization
		/// </summary>
		[XmlRoot("database")]
		public class Cards
		{
			[XmlArray("cards"), XmlArrayItem("card")]
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

		/// <summary>
		/// Load database
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <returns></returns>
		public static IEnumerable<Card> Load(string xmlPath)
		{
			try
			{
				using (var stream = new FileStream(xmlPath, FileMode.Open))
				{
					var serializer = new XmlSerializer(typeof(Cards));
					var cards = serializer.Deserialize(stream) as Cards;
					return cards.cards;
				}
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when loading database", ex);
			}
		}

		/// <summary>
		/// Save Database
		/// </summary>
		/// <param name="cardsList"></param>
		/// <param name="xmlPath"></param>
		public static void Save(IEnumerable<Card> cardsList, string xmlPath)
		{
			try
			{

				using (var stream = new FileStream(xmlPath, FileMode.Create))
				{
					var serializer = new XmlSerializer(typeof(Cards));
					var cards = new Cards(cardsList);
					serializer.Serialize(stream, cards);
				}

			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when saving database", ex);
			}
		}

	}
}
