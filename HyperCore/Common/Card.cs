using System.Xml;
using System.Xml.Serialization;
namespace HyperCore.Common
{
	/// <summary>
	/// Card class that contains all basic info
	/// </summary>
	public class Card
	{
		[XmlAttribute("id")]
		/// <summary>
		/// English WotcID of the card
		/// (use '|' as separator for dual, e.g. '12345|67890')
		/// </summary>
		public string ID { get; set; }

		[XmlAttribute("zid")]
		/// <summary>
		/// Foreign WotcID of the card
		/// (use '|' as separator for dual, e.g. '12345|67890')
		/// </summary>
		public string zID { get; set; }

		[XmlAttribute("var")]
		/// <summary>
		/// Variation of the card(for basic land card)
		/// (in the format of '(1:373546)(2:373609)(3:373683)(4:373746)')
		/// </summary>
		public string Var { get; set; }

		[XmlAttribute("name")]
		/// <summary>
		/// English name of the card
		/// (use '|' as separator for dual, e.g. 'ABC|DEF')
		/// </summary>
		public string Name { get; set; }

		[XmlAttribute("zname")]
		/// <summary>
		/// Foreign name of the card
		/// (use '|' as separator for dual, e.g. 'ABC|DEF')
		/// </summary>
		public string zName { get; set; }

		[XmlAttribute("set")]
		/// <summary>
		/// Full english set name of the card
		/// (use '|' as separator for dual, e.g. 'ABC|DEF')
		/// </summary>
		public string Set { get; set; }

		[XmlAttribute("setcode")]
		/// <summary>
		/// Setcode in capital
		/// </summary>
		public string SetCode { get; set; }

		[XmlAttribute("color")]
		/// <summary>
		/// Full english color name of the card
		/// (use use ' ' as separator for multi-color, e.g. 'Blue Red')
		/// (use use '|' as separator for dual, e.g. 'Blue|Black')
		/// </summary>
		public string Color { get; set; }

		[XmlAttribute("colorcode")]
		/// <summary>
		/// Colorcode in capital
		/// (no separator needed for multi-color, e.g. 'UR')
		/// (use use '|' as separator for dual, e.g. 'U|B')
		/// </summary>
		public string ColorCode { get; set; }

		[XmlAttribute("cost")]
		/// <summary>
		/// Cost of the card
		/// (use '{}' for each mana symbol, e.g. '{3}{B}{R}')
		/// (bracket hybrid mana symbol as one, e.g. '{WU}')
		/// (use use '|' as separator for dual, e.g. '{1}{W}|{2}{G}{G}')
		/// </summary>
		public string Cost { get; set; }

		[XmlAttribute("cmc")]
		/// <summary>
		/// Converted mana cost of the card
		/// (use use '|' as separator for dual, e.g. '3|2')
		/// </summary>
		public string CMC { get; set; }

		[XmlAttribute("type")]
		/// <summary>
		/// Type of the card
		/// (use use '|' as separator for dual, e.g. 'Creature — Human Advisor|Creature — Human Mutant')
		/// </summary>
		public string Type { get; set; }

		[XmlAttribute("ztype")]
		/// <summary>
		/// Type of the card in foreign
		/// (use use '|' as separator for dual, e.g. 'Creature — Human Advisor|Creature — Human Mutant')
		/// </summary>
		public string zType { get; set; }

		[XmlAttribute("typecode")]
		/// <summary>
		/// Typecode in capital
		/// (no separator needed for multi-type, e.g. 'AC')
		/// (use use '|' as separator for dual, e.g. 'C|C')
		/// </summary>
		public string TypeCode { get; set; }

		[XmlAttribute("pow")]
		/// <summary>
		/// Power of the card(creature)
		/// (use use '|' as separator for dual, e.g. '1|3')
		/// </summary>
		public string Pow { get; set; }

		[XmlAttribute("tgh")]
		/// <summary>
		/// Toughness of the card(creature)
		/// (use use '|' as separator for dual, e.g. '1|3')
		/// </summary>
		public string Tgh { get; set; }

		[XmlAttribute("loyalty")]
		/// <summary>
		/// Loyalty of the card(planeswalker)
		/// (use use '|' as separator for dual, e.g. '3|0')
		/// </summary>
		public string Loyalty { get; set; }

		[XmlAttribute("text")]
		/// <summary>
		/// English text of the card
		/// </summary>
		public string Text { get; set; }

		[XmlAttribute("ztext")]
		/// <summary>
		/// Foreign text of the card
		/// </summary>
		public string zText { get; set; }

		[XmlAttribute("flavor")]
		/// <summary>
		/// English flavor of the card
		/// </summary>
		public string Flavor { get; set; }

		[XmlAttribute("zflavor")]
		/// <summary>
		/// Foreign flavor of the card
		/// </summary>
		public string zFlavor { get; set; }

		[XmlAttribute("artist")]
		/// <summary>
		/// Artist name of the card
		/// </summary>
		public string Artist { get; set; }

		[XmlAttribute("rarity")]
		/// <summary>
		/// Rarity of the card
		/// </summary>
		public string Rarity { get; set; }

		[XmlAttribute("raritycode")]
		/// <summary>
		/// Raritycode of the card
		/// </summary>
		public string RarityCode { get; set; }

		[XmlAttribute("number")]
		/// <summary>
		/// Number of the card
		/// (use use '|' as separator for dual, e.g. '121a|121b')
		/// </summary>
		public string Number { get; set; }

		[XmlAttribute("rulings")]
		/// <summary>
		/// Rulings of the card
		/// </summary>
		public string Rulings { get; set; }

		[XmlAttribute("legality")]
		/// <summary>
		/// Legality of the card
		/// </summary>
		public string Legality { get; set; }

		[XmlAttribute("rating")]
		/// <summary>
		/// Community rating of the card
		/// </summary>
		public string Rating { get; set; }

		public string ColorBside;

	}
}
