using HyperCore.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HyperCore.IO
{
	public class Extern
	{
		public class VPT
		{
			[XmlType("card")]
			public class VPTCard
			{
				[XmlAttribute("set")]
				public string SetCode { get; set; }

				[XmlAttribute("lang")]
				public string Lang { get; set; }

				[XmlAttribute("ver")]
				public string Var { get; set; }

				[XmlAttribute("count")]
				public int Count { get; set; }

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
				public string Name { get; set; }

				[XmlElement("card")]
				public List<VPTCard> Cards { get; set; }

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
				public string ID { get; set; }

				[XmlElement("item")]
				public List<VPTItem> items { get; set; }

				/// <summary>
				/// Initializes a new instance of the VPTSection class.
				/// </summary>
				public VPTSection(string iD, List<VPTItem> items)
				{
					ID = iD;
					this.items = items;
				}

				/// <summary>
				/// Initializes a new instance of the VPTSection class.
				/// </summary>
				public VPTSection()
				{
					ID = String.Empty;
					items = new List<VPTItem>();
				}
			}

			[XmlRoot("deck")]
			public class VPTDeck
			{
				[XmlAttribute("game")]
				public string Game { get; set; }

				[XmlAttribute("mode")]
				public string Mode { get; set; }

				[XmlAttribute("format")]
				public string Format { get; set; }

				[XmlAttribute("name")]
				public string Name { get; set; }

				[XmlElement("section")]
				public List<VPTSection> Sections { get; set; }

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

			public static VPTDeck Open(string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var serializer = new XmlSerializer(typeof(VPTDeck));
						var data = serializer.Deserialize(stream) as VPTDeck;
						return data;

					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when opening vpt file", ex);
				}
			}

			public static void Export(string path)
			{
				try
				{

					using (var stream = new FileStream(path, FileMode.Create))
					{
						var serializer = new XmlSerializer(typeof(VPTDeck));
						var nas = new XmlSerializerNamespaces();
						nas.Add(string.Empty, string.Empty);
						serializer.Serialize(stream, new VPTDeck(), nas);
					}

				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting vpt file", ex);
				}
			}
		}
	}
}
