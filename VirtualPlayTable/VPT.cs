using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperKore.IO;
using System.Xml.Serialization;
using System.IO;
using HyperKore.Xception;
using HyperKore.Common;
using HyperKore.Utility;

namespace VirtualPlayTable
{
	public class VPT : IDeckReader, IDeckWriter
	{
		[XmlType("card")]
		public class VPTCard
		{
			[XmlAttribute("set")]
			public string SetCode
			{
				get;
				set;
			}

			[XmlAttribute("lang")]
			public string Lang
			{
				get;
				set;
			}

			[XmlAttribute("ver")]
			public string Var
			{
				get;
				set;
			}

			[XmlAttribute("count")]
			public int Count
			{
				get;
				set;
			}

			/// <summary>
			/// Initializes a new instance of the VPTCard class.
			/// </summary>
			public VPTCard(string setCode, string lang, string @var, int count)
			{
				SetCode = setCode;
				Lang = lang;
				Var = @var;
				Count = count;
			}

			/// <summary>
			/// Initializes a new instance of the VPTCard class.
			/// </summary>
			public VPTCard()
			{
				SetCode = String.Empty;
				Lang = String.Empty;
				Var = String.Empty;
				Count = 0;
			}
		}

		[XmlType("item")]
		public class VPTItem
		{
			[XmlAttribute("id")]
			public string Name
			{
				get;
				set;
			}

			[XmlElement("card")]
			public List<VPTCard> Cards
			{
				get;
				set;
			}

			/// <summary>
			/// Initializes a new instance of the VPTItem class.
			/// </summary>
			public VPTItem(string name, List<VPTCard> cards)
			{
				Name = name;
				Cards = cards;
			}

			/// <summary>
			/// Initializes a new instance of the VPTItem class.
			/// </summary>
			public VPTItem()
			{
				Name = String.Empty;
				Cards = new List<VPTCard>();
			}
		}

		[XmlType("section")]
		public class VPTSection
		{
			[XmlAttribute("id")]
			public string ID
			{
				get;
				set;
			}

			[XmlElement("item")]
			public List<VPTItem> Items
			{
				get;
				set;
			}

			/// <summary>
			/// Initializes a new instance of the VPTSection class.
			/// </summary>
			public VPTSection(string iD, List<VPTItem> items)
			{
				ID = iD;
				this.Items = items;
			}

			/// <summary>
			/// Initializes a new instance of the VPTSection class.
			/// </summary>
			public VPTSection()
			{
				ID = String.Empty;
				Items = new List<VPTItem>();
			}
		}

		[XmlRoot("deck")]
		public class VPTDeck
		{
			[XmlAttribute("game")]
			public string Game
			{
				get;
				set;
			}

			[XmlAttribute("mode")]
			public string Mode
			{
				get;
				set;
			}

			[XmlAttribute("format")]
			public string Format
			{
				get;
				set;
			}

			[XmlAttribute("name")]
			public string Name
			{
				get;
				set;
			}

			[XmlElement("section")]
			public List<VPTSection> Sections
			{
				get;
				set;
			}

			/// <summary>
			/// Initializes a new instance of the VPTDeck class.
			/// </summary>
			public VPTDeck(string game, string mode, string format, string name, List<VPTSection> sections)
			{
				Game = game;
				Mode = mode;
				Format = format;
				Name = name;
				Sections = sections;
			}

			/// <summary>
			/// Initializes a new instance of the VPTDeck class.
			/// </summary>
			public VPTDeck()
			{
				Game = String.Empty;
				Mode = String.Empty;
				Format = String.Empty;
				Name = String.Empty;
				Sections = new List<VPTSection>();
			}
		}


		public HyperKore.Common.Deck Read(Stream input)
		{
			throw new NotImplementedException();
		}


		public bool Write(HyperKore.Common.Deck deck, Stream output)
		{
			throw new NotImplementedException();
		}

		private VPTDeck Open(Stream input)
		{
			try
			{
				var serializer = new XmlSerializer(typeof(VPTDeck));
				var data = serializer.Deserialize(input) as VPTDeck;
				return data;
			}
			catch (Exception ex)
			{
				throw new IOXception("IO Error happended when opening vpt file", ex);
			}
		}

		private void Export(VPTDeck deck, Stream output)
		{
			try
			{

				var serializer = new XmlSerializer(typeof(VPTDeck));
				var nas = new XmlSerializerNamespaces();
				nas.Add(string.Empty, string.Empty);
				serializer.Serialize(output, deck, nas);

			}
			catch (Exception ex)
			{
				throw new IOXception("IO Error happended when exporting vpt file", ex);
			}
		}

		private Card ConvertToCard(VPT.VPTItem item, IEnumerable<Card> database)
		{
			var res = database.First(c => item.Cards[0].SetCode == c.SetCode && item.Name == c.GetLegalName());
			if (res != null)
			{
				return res;
			}
			else
			{
				//throw new CardMissingException("Card not found when loading vpt deck.", null, item.Name, item.Cards[0].SetCode);
				return null;
			}
		}

		private VPTDeck ConvertToVDeck(Deck deck)
		{
			try
			{
				VPTSection sectionM = new VPTSection();
				sectionM.ID = "main";
				


				VPTSection sectionS = new VPTSection();
				sectionS.ID = "sideboard";
				

				VPTDeck vdeck = new VPTDeck("mtg", deck.Mode.ToString(), deck.Format.ToString(), deck.Name, new List<VPTSection>()
				{
					sectionM, sectionS
				});

				return vdeck;
			}
			catch
			{
				throw;
			}
		}
	}
}
