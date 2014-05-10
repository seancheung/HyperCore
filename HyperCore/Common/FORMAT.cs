using System.Collections.Generic;
namespace HyperCore.Common
{
	/// <summary>
	/// Format of the game
	/// </summary>
	public enum FORMAT
	{
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
		/// <summary>
		/// Type of the format
		/// </summary>
		public FORMAT Type { get; private set; }

		/// <summary>
		/// A list that contains names of all legal sets
		/// </summary>
		public IEnumerable<string> LegalSets { get; private set; }

		/// <summary>
		/// A list that contains names of all banned cards
		/// </summary>
		public IEnumerable<string> BannedCards { get; private set; }

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
	}
}
