using HyperCore.Utilities;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace HyperCore.Common
{
	[XmlRoot("hyperdeck")]
	public class Deck
	{
		[XmlAttribute("name")]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Use ObservableDictionary<TKey,TValue> if data binding is used, 
		/// otherwise a simple Dictionary<TKey,TValue> is proper.
		/// Provide a comparer on instantiating
		/// </summary>
		[XmlArray("mainboard"), XmlArrayItem("card")]
		public ObservableDictionary<Card, int> MainBoard
		{
			get;
			set;
		}
		/// <summary>
		/// Use ObservableDictionary<TKey,TValue> if data binding is used, 
		/// otherwise a simple Dictionary<TKey,TValue> is proper.
		/// Provide a comparer on instantiating
		/// </summary>
		[XmlArray("sideboard"), XmlArrayItem("card")]
		public ObservableDictionary<Card, int> SideBoard
		{
			get;
			set;
		}
		public string Comment
		{
			get;
			set;
		}

		[XmlAttribute("format")]
		public FORMAT Format
		{
			get;
			set;
		}

		[XmlAttribute("type")]
		public MODE Mode
		{
			get;
			set;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the Deck class.
		/// </summary>
		public Deck()
		{
			Name = String.Empty;
			MainBoard = new ObservableDictionary<Card, int>(new Comparer.CardComparer());
			SideBoard = new ObservableDictionary<Card, int>(new Comparer.CardComparer());
			Comment = String.Empty;
			Format = FORMAT.Default;
			Mode = MODE.Default;
		}
	}
}
