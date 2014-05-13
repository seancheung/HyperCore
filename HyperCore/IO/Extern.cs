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

			/// <summary>
			/// Open Vpt deck file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
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

			/// <summary>
			/// Export as VPT deck file
			/// </summary>
			/// <param name="deck"></param>
			/// <param name="path"></param>
			public static void Export(VPTDeck deck, string path)
			{
				try
				{

					using (var stream = new FileStream(path, FileMode.Create))
					{
						var serializer = new XmlSerializer(typeof(VPTDeck));
						var nas = new XmlSerializerNamespaces();
						nas.Add(string.Empty, string.Empty);
						serializer.Serialize(stream, deck, nas);
					}

				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting vpt file", ex);
				}
			}
		}

		public class MWS
		{
			public class MWSCard
			{
				public int Count { get; set; }

				public string SetCode { get; set; }

				public string Name { get; set; }

				public string Var { get; set; }

				/// <summary>
				/// Initializes a new instance of the MWSCard class with parameters.
				/// </summary>
				public MWSCard(string setCode, string name, int count = 1, string @var = null)
				{
					Count = count;
					SetCode = setCode;
					Name = name;
					Var = @var;
				}

				public MWSCard()
				{

				}
			}

			public class MWSDeck
			{
				public string Name { get; set; }

				public string Comment { get; set; }

				public List<MWSCard> MainBoardLands { get; set; }

				public List<MWSCard> MainBoardSpells { get; set; }

				public List<MWSCard> SideBoard { get; set; }

				/// <summary>
				/// Initializes a new instance of the MWSDeck class with parameters.
				/// </summary>
				public MWSDeck(string name, IEnumerable<MWSCard> mainBoardLands, IEnumerable<MWSCard> mainBoardSpells, IEnumerable<MWSCard> sideBoard, string comment = "")
				{
					Name = name;
					Comment = comment;
					MainBoardLands = new List<MWSCard>(mainBoardLands);
					MainBoardSpells = new List<MWSCard>(mainBoardSpells);
					SideBoard = new List<MWSCard>(sideBoard);
				}

				/// <summary>
				/// Initializes a new instance of the MWSDeck class.
				/// </summary>
				public MWSDeck()
				{

				}
			}

			/// <summary>
			/// Open MWS file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			public static MWSDeck Open(string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var sr = new StreamReader(stream);
						sr.BaseStream.Seek(0L, SeekOrigin.Begin);
						MWSDeck deck = new MWSDeck();

						int partID = 1; // 0 - comment,1 - mainlands, 2 - mainspells, 3 - side

						var line = sr.ReadLine();
						while (line != null)
						{
							if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
							{
								if (line.StartsWith("SB:"))
								{
									partID = 3;
								}

								switch (partID)
								{
									case 0:
										if (deck.Comment == null)
										{
											deck.Comment = line;
										}
										else
										{
											deck.Comment += line;
										}
										break;
									case 1:
										var idxa = line.IndexOf("[");
										var idxb = line.IndexOf("]");

										if (idxa > 0 && idxb > idxa)
										{
											MWSCard card = new MWSCard();

											var count = line.Remove(idxa).Trim();
											card.Count = Convert.ToInt32(count);

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa) : string.Empty;
											card.SetCode = setcode.Trim();

											var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
											card.Name = name.Trim();

											deck.MainBoardSpells.Add(card);
										}
										break;
									case 2:

										idxa = line.IndexOf("[");
										idxb = line.IndexOf("]");

										if (idxa > 0 && idxb > idxa)
										{
											MWSCard card = new MWSCard();

											var count = line.Remove(idxa).Trim();
											card.Count = Convert.ToInt32(count);

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa) : string.Empty;
											card.SetCode = setcode.Trim();

											var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
											card.Name = name.Trim();

											var idxc = line.IndexOf("(");
											var idxd = line.IndexOf(")");
											if (idxc > 0 && idxd > idxc)
											{
												var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc) : string.Empty;
												card.Var = var.Trim();
												card.Name = name.Remove(name.IndexOf("("));
											}

											deck.MainBoardLands.Add(card);
										}
										break;
									case 3:
										idxa = line.IndexOf("[");
										idxb = line.IndexOf("]");

										if (idxa > 0 && idxb > idxa)
										{
											MWSCard card = new MWSCard();

											var count = line.Remove(idxa).Replace("SB:", string.Empty).Trim();
											card.Count = Convert.ToInt32(count);

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa) : string.Empty;
											card.SetCode = setcode.Trim();

											var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
											card.Name = name.Trim();

											var idxc = line.IndexOf("(");
											var idxd = line.IndexOf(")");
											if (idxc > 0 && idxd > idxc)
											{
												var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc) : string.Empty;
												card.Var = var.Trim();
												card.Name = name.Remove(name.IndexOf("("));
											}

											deck.SideBoard.Add(card);
										}
										break;
									default:
										break;
								}

							}
							else
							{
								if (line.Contains("Comments"))
								{
									partID = 0;
								}
								else if (line.Contains("Sideboard"))
								{
									partID = 3;
								}
								else if (line.Contains("Lands"))
								{
									partID = 2;
								}
								else
								{
									partID = 1;
								}
							}

							line = sr.ReadLine();
						}

						return deck;
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when opening mws file", ex);
				}
			}

			/// <summary>
			/// Export as MWS file
			/// </summary>
			/// <param name="deck"></param>
			/// <param name="path"></param>
			public static void Export(MWSDeck deck, string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						var sw = new StreamWriter(stream);

						sw.WriteLine("// Comments\n");
						sw.WriteLine(deck.Name);
						sw.WriteLine(deck.Comment);

						sw.WriteLine("\r\n// Lands\n");
						deck.MainBoardLands.FindAll(c => !string.IsNullOrWhiteSpace(c.Var)).ForEach(c => sw.WriteLine(string.Format("{0} [{1}] {2} (1)", c.Count, c.SetCode, c.Name, c.Var)));
						deck.MainBoardLands.FindAll(c => string.IsNullOrWhiteSpace(c.Var)).ForEach(c => sw.WriteLine(string.Format("{0} [{1}] {2}", c.Count, c.SetCode, c.Name)));

						sw.WriteLine("\r\n// Spells\n");
						deck.MainBoardSpells.ForEach(c => sw.WriteLine("{0} [{1}] {2}", c.Count, c.SetCode, c.Name));

						sw.WriteLine("\r\n// Sideboard\n");
						deck.SideBoard.ForEach(c => sw.WriteLine("SB: {0} [{1}] {2}", c.Count, c.SetCode, c.Name));
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting mws file", ex);
				}
			}
		}
	}
}
