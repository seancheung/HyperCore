
namespace HyperCore.Common
{
	public enum COLOR
	{
		Red,
		Blue,
		Green,
		Black,
		White
	}

	public enum RARITY
	{
		Common,
		Uncommon,
		Rare,
		Mythic
	}

	public enum TYPE
	{
		Artifact,
		Equipment,
		Basic,
		Conspiracy,
		Creature,
		Enchantment,
		Aura,
		Instant,
		Land,
		Legendary,
		Ongoing,
		Phenomenon,
		Plane,
		Planeswalker,
		Scheme,
		Snow,
		Sorcery,
		Tribal,
		Vanguard,
		World
	}

	/// <summary>
	/// All supported filetypes
	/// </summary>
	public enum FILETYPE
	{
		VirtualPlayTable,
		MagicWorkstation,
		Mage,
		MagicOnline,
		HyperDeck
	}

	/// <summary>
	/// Available languages of the card info
	/// </summary>
	public enum LANGUAGE
	{
		ChineseSimplified,
		ChineseTraditional,
		German,
		French,
		Italian,
		Japanese,
		Korean,
		Portuguese,
		Russian,
		Spanish,
		English
	}

	/// <summary>
	/// Deck Type
	/// </summary>
	public enum MODE
	{
		Default,
		Constructed,
		Block,
		Sealed,
		Draft
	}

	/// <summary>
	/// Available websites for grabbing data
	/// </summary>
	public enum WEBSITE
	{
		gatherer,
		magiccards,
		magicspoiler,
		iplaymtg
	}
}
