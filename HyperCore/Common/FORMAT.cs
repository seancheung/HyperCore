using System.Collections.Generic;
using System.Xml.Serialization;

namespace HyperCore.Common
{
	/// <summary>
	/// Format of the deck
	/// </summary>
	public enum FORMAT
	{
		Default,
		Standard,
		Modern,
		Extended,
		Block,
		Vintage,
		Legacy,
		Classic,
		Commander
	}

	/// <summary>
	/// Game format that contians legal sets and banned cards
	/// </summary>
	public class Format
	{
		[XmlAttribute("type")]
		/// <summary>
		/// Type of the format
		/// </summary>
		public FORMAT Type { get; set; }

		[XmlArray("sets"), XmlArrayItem("set")]
		/// <summary>
		/// A list that contains names of all legal sets
		/// </summary>
		public List<string> LegalSets { get; set; }

		[XmlArray("banned"), XmlArrayItem("name")]
		/// <summary>
		/// A list that contains names of all banned cards
		/// </summary>
		public List<string> BannedCards { get; set; }

		/// <summary>
		/// Initialize with parameters
		/// </summary>
		/// <param name="format">Format name</param>
		/// <param name="sets">Legal sets</param>
		/// <param name="cards">Banned cards</param>
		public Format(FORMAT format, IEnumerable<string> sets, IEnumerable<string> cards)
		{
			Type = format;
			LegalSets = new List<string>(sets);
			BannedCards = new List<string>(cards);
		}

		public Format()
		{
			LegalSets = new List<string>();
			BannedCards = new List<string>();
		}
	}
}
