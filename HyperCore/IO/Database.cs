using HyperCore.Common;
using HyperCore.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HyperCore.IO
{
	public class Database
	{
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
				using (var stream = new FileStream(xmlPath, FileMode.Append))
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
