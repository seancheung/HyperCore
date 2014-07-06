using HyperCore.Common;
using HyperCore.Exceptions;
using HyperCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HyperCore.IO
{
	public class Extern
	{
		public string DataPath
		{
			get;
			private set;
		}

		public class VPT
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

			/// <summary>
			/// Open Vpt deck file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			internal VPTDeck Open(string path)
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
			internal void Export(VPTDeck deck, string path)
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

		internal class MWS
		{
			public class MWSCard
			{
				public int Count
				{
					get;
					set;
				}

				public string SetCode
				{
					get;
					set;
				}

				public string Name
				{
					get;
					set;
				}

				public string Var
				{
					get;
					set;
				}

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

				/// <summary>
				/// Initializes a new instance of the MWSCard class.
				/// </summary>
				public MWSCard()
				{
					Count = 0;
					SetCode = String.Empty;
					Name = String.Empty;
					Var = String.Empty;
				}
			}

			public class MWSDeck
			{
				public string Name
				{
					get;
					set;
				}

				public string Comment
				{
					get;
					set;
				}

				public List<MWSCard> MainBoardLands
				{
					get;
					set;
				}

				public List<MWSCard> MainBoardSpells
				{
					get;
					set;
				}

				public List<MWSCard> SideBoard
				{
					get;
					set;
				}

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
					Name = String.Empty;
					Comment = String.Empty;
					MainBoardLands = new List<MWSCard>();
					MainBoardSpells = new List<MWSCard>();
					SideBoard = new List<MWSCard>();
				}
			}

			/// <summary>
			/// Open MWS file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			public MWSDeck Open(string path)
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

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
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

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
											card.SetCode = setcode.Trim();

											var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
											card.Name = name.Trim();

											var idxc = line.IndexOf("(");
											var idxd = line.IndexOf(")");
											if (idxc > 0 && idxd > idxc)
											{
												var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc - 1) : string.Empty;
												card.Var = var.Trim();
												card.Name = name.Remove(name.IndexOf("(")).Trim();
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

											var setcode = idxb - idxa > 1 ? line.Substring(idxa + 1, idxb - idxa - 1) : string.Empty;
											card.SetCode = setcode.Trim();

											var name = idxb < line.Length ? line.Substring(idxb + 1) : string.Empty;
											card.Name = name.Trim();

											var idxc = line.IndexOf("(");
											var idxd = line.IndexOf(")");
											if (idxc > 0 && idxd > idxc)
											{
												var var = idxd - idxc > 1 ? line.Substring(idxc + 1, idxd - idxc - 1) : string.Empty;
												card.Var = var.Trim();
												card.Name = name.Remove(name.IndexOf("(")).Trim();
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
								else if (line.Contains("Spells"))
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
			public void Export(MWSDeck deck, string path)
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

						sw.Flush();
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting mws file", ex);
				}
			}
		}

		internal class MO
		{
			public class MOCard
			{
				public int Count
				{
					get;
					set;
				}

				public string Name
				{
					get;
					set;
				}
			}

			public class MODeck
			{
				public string Name
				{
					get;
					set;
				}

				public List<MOCard> MainBoard
				{
					get;
					set;
				}

				public List<MOCard> SideBoard
				{
					get;
					set;
				}

				/// <summary>
				/// Initializes a new instance of the MODeck class.
				/// </summary>
				public MODeck(IEnumerable<MOCard> mainBoard, IEnumerable<MOCard> sideBoard, string name = "")
				{
					Name = name;
					MainBoard = new List<MOCard>(mainBoard);
					SideBoard = new List<MOCard>(sideBoard);
				}

				/// <summary>
				/// Initializes a new instance of the MODeck class.
				/// </summary>
				public MODeck()
				{
					Name = String.Empty;
					MainBoard = new List<MOCard>();
					SideBoard = new List<MOCard>();
				}
			}

			/// <summary>
			/// Export as mo file
			/// </summary>
			/// <param name="deck"></param>
			/// <param name="path"></param>
			public void Export(MODeck deck, string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						var sw = new StreamWriter(stream);

						deck.MainBoard.ForEach(c => sw.WriteLine(String.Format("{0} {1}", c.Count, c.Name)));

						sw.WriteLine("Sideboard");

						deck.SideBoard.ForEach(c => sw.WriteLine(String.Format("{0} {1}", c.Count, c.Name)));

						sw.Flush();
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting mo file", ex);
				}
			}

			/// <summary>
			/// Open MO file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			public MODeck Open(string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var sr = new StreamReader(stream);
						sr.BaseStream.Seek(0L, SeekOrigin.Begin);
						MODeck deck = new MODeck();

						var line = sr.ReadLine();
						int partID = 0; // 0 - main, 1 - side
						while (line != null)
						{

							if (line.Contains("Sideboard"))
							{
								partID = 1;
							}
							else if (!string.IsNullOrWhiteSpace(line))
							{
								MOCard card = new MOCard();

								if (partID == 0)
								{
									var idx = line.IndexOf(" ");
									if (idx > 0)
									{
										var count = line.Remove(idx).Trim();
										var name = line.Substring(idx).Trim();
										int cnt = 0;
										if (Int32.TryParse(count, out cnt))
										{
											card.Count = cnt;
											card.Name = name;

											deck.MainBoard.Add(card);
										}

									}

								}
								else
								{
									var idx = line.IndexOf(" ");
									if (idx > 0)
									{
										var count = line.Remove(idx).Trim();
										var name = line.Substring(idx).Trim();
										int cnt;
										if (Int32.TryParse(count, out cnt))
										{
											card.Count = cnt;
											card.Name = name;

											deck.SideBoard.Add(card);
										}

									}
								}
							}

							line = sr.ReadLine();
						}

						return deck;
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when opening mo file", ex);
				}

			}
		}

		internal class MAGE
		{
			public class MAGECard
			{
				public int Count
				{
					get;
					set;
				}

				public string SetCode
				{
					get;
					set;
				}

				public string Number
				{
					get;
					set;
				}

				public string Name
				{
					get;
					set;
				}

			}

			public class MAGEDeck
			{
				public string Name
				{
					get;
					set;
				}

				public List<MAGECard> MainBoard
				{
					get;
					set;
				}

				public List<MAGECard> SideBoard
				{
					get;
					set;
				}

				/// <summary>
				/// Initializes a new instance of the MageDeck class.
				/// </summary>
				public MAGEDeck(IEnumerable<MAGECard> mainBoard, IEnumerable<MAGECard> sideBoard, string name = "")
				{
					Name = name;
					MainBoard = new List<MAGECard>(mainBoard);
					SideBoard = new List<MAGECard>(sideBoard);
				}

				/// <summary>
				/// Initializes a new instance of the MageDeck class.
				/// </summary>
				public MAGEDeck()
				{
					Name = String.Empty;
					MainBoard = new List<MAGECard>();
					SideBoard = new List<MAGECard>();
				}
			}

			/// <summary>
			/// Export Mage file
			/// </summary>
			/// <param name="deck"></param>
			/// <param name="path"></param>
			public void Export(MAGEDeck deck, string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						var sw = new StreamWriter(stream);

						sw.WriteLine("NAME: " + deck.Name);

						deck.MainBoard.ForEach(c => sw.WriteLine(String.Format("{0} [{1}:{2}] {3}", c.Count, c.SetCode, c.Number, c.Name)));

						deck.SideBoard.ForEach(c => sw.WriteLine(String.Format("SB: {0} [{1}:{2}] {3}", c.Count, c.SetCode, c.Number, c.Name)));

						sw.Flush();
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting mage file", ex);
				}
			}

			/// <summary>
			/// Open Mage file
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			public MAGEDeck Open(string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var sr = new StreamReader(stream);
						sr.BaseStream.Seek(0L, SeekOrigin.Begin);
						MAGEDeck deck = new MAGEDeck();

						var line = sr.ReadLine();

						while (line != null)
						{
							if (line.Contains("["))
							{
								MAGECard card = new MAGECard();

								if (line.Contains("SB:"))
								{
									var idxa = line.IndexOf("[");
									var idxb = line.IndexOf("]");

									if (idxa > 0 && idxb > idxa)
									{
										var idxc = line.IndexOf(":", idxa);
										var idxd = line.IndexOf(":");
										if (idxc > idxa && idxc < idxb)
										{
											var count = line.Substring(idxd + 1, idxa - idxd - 1).Trim();
											var setcode = line.Substring(idxa + 1, idxc - idxa - 1).Trim();
											var number = line.Substring(idxc + 1, idxb - idxc - 1).Trim();
											var name = line.Substring(idxb + 1).Replace("SB:", string.Empty).Trim();
											int cnt;

											if (Int32.TryParse(count, out cnt))
											{
												card.Name = name;
												card.Count = cnt;
												card.Number = number;
												card.SetCode = setcode;

												deck.SideBoard.Add(card);
											}

										}

									}
								}
								else
								{
									var idxa = line.IndexOf("[");
									var idxb = line.IndexOf("]");

									if (idxa > 0 && idxb > idxa)
									{
										var idxc = line.IndexOf(":", idxa);
										if (idxc > idxa && idxc < idxb)
										{
											var count = line.Remove(idxa).Trim();
											var setcode = line.Substring(idxa + 1, idxc - idxa - 1).Trim();
											var number = line.Substring(idxc + 1, idxb - idxc - 1).Trim();
											var name = line.Substring(idxb + 1).Trim();
											int cnt;

											if (Int32.TryParse(count, out cnt))
											{
												card.Name = name;
												card.Count = cnt;
												card.Number = number;
												card.SetCode = setcode;

												deck.MainBoard.Add(card);
											}

										}

									}

								}
							}
							else if (!string.IsNullOrWhiteSpace(line))
							{
								deck.Name = line.Replace("NAME:", string.Empty).Replace("name:", string.Empty).Trim();
							}

							line = sr.ReadLine();
						}

						return deck;
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when opening mage file", ex);
				}
			}
		}

		internal class Hyper
		{
			public class HyperDeck
			{
				public string Name
				{
					get;
					set;
				}
				public string Comment
				{
					get;
					set;
				}
				public FORMAT Format
				{
					get;
					set;
				}
				public MODE Mode
				{
					get;
					set;
				}

				public List<string> MainBoard
				{
					get;
					set;
				}
				public List<string> SideBoard
				{
					get;
					set;
				}

				/// <summary>
				/// Initializes a new instance of the HyperDeck class with parameters.
				/// </summary>
				/// <param name="name"></param>
				/// <param name="comment"></param>
				/// <param name="format"></param>
				/// <param name="mode"></param>
				/// <param name="mainBoard"></param>
				/// <param name="sideBoard"></param>
				public HyperDeck(string name, string comment, FORMAT format, MODE mode, IEnumerable<string> mainBoard, IEnumerable<string> sideBoard)
				{
					Name = name;
					Comment = comment;
					Format = format;
					Mode = mode;
					MainBoard = new List<string>(mainBoard);
					SideBoard = new List<string>(sideBoard);
				}

				/// <summary>
				/// Initializes a new instance of the HyperDeck class.
				/// </summary>
				public HyperDeck()
				{

				}
			}

			public HyperDeck Open(string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Open))
					{
						var sr = new StreamReader(stream);
						var json = sr.ReadToEnd();
						return new JsonIO().ReadDeck(json);
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when opening xdeck file", ex);
				}
			}

			internal void Export(HyperDeck xdeck, string path)
			{
				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						var sw = new StreamWriter(stream);
						var data = new JsonIO().Convert(xdeck);
						sw.Write(data);
						sw.Flush();
					}
				}
				catch (Exception ex)
				{
					throw new IOFileException(path, "IO Error happended when exporting xdeck file", ex);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the Extern class.
		/// </summary>
		public Extern(string dataPath)
		{
			DataPath = dataPath;
		}

		/// <summary>
		/// Open deck files
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Deck Open(string path)
		{
			Deck deck = new Deck();
			var ext = path.Substring(path.LastIndexOf(".") + 1).ToUpper();

			if (ext == FILETYPE.VirtualPlayTable.GetFileExt().ToUpper())
			{
				try
				{
					var database = Database.Instance.LoadCards(DataPath);
					var vdeck = new VPT().Open(path);
					vdeck.Sections[0].Items.ForEach(i => deck.MainBoard.Add(ConvertToCard(i, database), i.Cards.Sum(c => c.Count)));
					vdeck.Sections[1].Items.ForEach(i => deck.SideBoard.Add(ConvertToCard(i, database), i.Cards.Sum(c => c.Count)));
					deck.Name = vdeck.Name;
					MODE mode;
					if (Enum.TryParse<MODE>(vdeck.Mode, true, out mode))
					{
						deck.Mode = mode;
					}
					FORMAT format;
					if (Enum.TryParse<FORMAT>(vdeck.Format, true, out format))
					{
						deck.Format = format;
					}

					return deck;
				}
				catch
				{
					throw;
				}
			}
			else if (ext == FILETYPE.MagicWorkstation.GetFileExt().ToUpper())
			{
				try
				{
					var database = Database.Instance.LoadCards(DataPath);
					var mdeck = new MWS().Open(path);
					mdeck.MainBoardLands.ForEach(c => deck.MainBoard.Add(ConvertToCard(c, database), c.Count));
					mdeck.MainBoardSpells.ForEach(c => deck.MainBoard.Add(ConvertToCard(c, database), c.Count));
					mdeck.SideBoard.ForEach(c => deck.SideBoard.Add(ConvertToCard(c, database), c.Count));
					deck.Name = mdeck.Name;
					deck.Comment = mdeck.Comment;

					return deck;
				}
				catch
				{
					throw;
				}

			}
			else if (ext == FILETYPE.Mage.GetFileExt().ToUpper())
			{
				try
				{
					var gdeck = new MAGE().Open(path);
					if (gdeck.MainBoard.Count + gdeck.SideBoard.Count == 0)
					{
						var odeck = new MO().Open(path);
						if (odeck.MainBoard.Count + odeck.SideBoard.Count == 0)
						{
							return null;
						}
						var database = Database.Instance.LoadCards(DataPath);
						odeck.MainBoard.ForEach(c => deck.MainBoard.Add(ConvertToCard(c, database), c.Count));
						odeck.SideBoard.ForEach(c => deck.SideBoard.Add(ConvertToCard(c, database), c.Count));
						deck.Name = odeck.Name;

						return deck;
					}
					else
					{
						var database = Database.Instance.LoadCards(DataPath);
						gdeck.MainBoard.ForEach(c => deck.MainBoard.Add(ConvertToCard(c, database), c.Count));
						gdeck.SideBoard.ForEach(c => deck.SideBoard.Add(ConvertToCard(c, database), c.Count));
						deck.Name = gdeck.Name;

						return deck;
					}
				}
				catch
				{
					throw;
				}

			}
			else if (ext == FILETYPE.HyperDeck.GetFileExt().ToUpper())
			{
				try
				{
					var xdeck = new Hyper().Open(path);
					var database = Database.Instance.LoadCards(DataPath);
					foreach (var str in xdeck.MainBoard)
					{
						var kv = ConvertToCardStack(str, database);
						deck.MainBoard.Add(kv.Key, kv.Value);
					}
					foreach (var str in xdeck.SideBoard)
					{
						var kv = ConvertToCardStack(str, database);
						deck.SideBoard.Add(kv.Key, kv.Value);
					}

					deck.Name = xdeck.Name;
					deck.Comment = xdeck.Comment;
					deck.Mode = xdeck.Mode;
					deck.Format = xdeck.Format;

					return deck;
				}
				catch
				{
					throw;
				}
			}

			else
			{
				throw new IOFileException(path, "File type not supported", null);
			}


		}

		/// <summary>
		/// Export deck to a specified filetype
		/// </summary>
		/// <param name="deck"></param>
		/// <param name="path"></param>
		/// <param name="fileType"></param>
		public void Export(Deck deck, string path, FILETYPE fileType)
		{
			try
			{
				switch (fileType)
				{
					case FILETYPE.VirtualPlayTable:
						new VPT().Export(ConvertToVDeck(deck), path);
						break;
					case FILETYPE.MagicWorkstation:
						new MWS().Export(ConvertToMDeck(deck), path);
						break;
					case FILETYPE.Mage:
						new MAGE().Export(ConvertToGDeck(deck), path);
						break;
					case FILETYPE.MagicOnline:
						new MO().Export(ConvertToODeck(deck), path);
						break;
					case FILETYPE.HyperDeck:
						new Hyper().Export(ConvertToXDeck(deck), path);
						break;
					default:
						throw new IOFileException(path, "File type not supported", null);
				}
			}
			catch
			{
				throw;
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
				throw new CardMissingException("Card not found when loading vpt deck.", null, item.Name, item.Cards[0].SetCode);
			}
		}

		private Card ConvertToCard(MWS.MWSCard card, IEnumerable<Card> database)
		{
			var res = database.First(c => card.SetCode == c.SetCode && card.Name == c.GetLegalName());
			if (res != null)
			{
				return res;
			}
			else
			{
				throw new CardMissingException("Card not found when loading vpt deck.", null, card.Name, card.SetCode);
			}
		}

		private Card ConvertToCard(MO.MOCard card, IEnumerable<Card> database)
		{
			var res = database.First(c => card.Name == c.GetLegalName());
			if (res != null)
			{
				return res;
			}
			else
			{
				throw new CardMissingException("Card not found when loading vpt deck.", null, card.Name, "Unknown");
			}
		}

		private Card ConvertToCard(MAGE.MAGECard card, IEnumerable<Card> database)
		{
			var res = database.First(c => card.Name == c.GetLegalName() && card.SetCode == c.SetCode);
			if (res != null)
			{
				return res;
			}
			else
			{
				throw new CardMissingException("Card not found when loading vpt deck.", null, card.Name, card.SetCode);
			}
		}

		private KeyValuePair<Card, int> ConvertToCardStack(string cardIDwithQnt, IEnumerable<Card> database)
		{
			var split = cardIDwithQnt.Split(new char[] { '#' });
			var card = database.First(c => c.ID == split[0]);
			int qnt = Int32.Parse(split[1]);

			return new KeyValuePair<Card, int>(card, qnt);
		}

		private VPT.VPTDeck ConvertToVDeck(Deck deck)
		{
			try
			{
				VPT.VPTSection sectionM = new VPT.VPTSection();
				sectionM.ID = "main";
				foreach (KeyValuePair<Card, int> kv in deck.MainBoard)
				{
					VPT.VPTCard card = new VPT.VPTCard(kv.Key.SetCode, LANGUAGE.English.GetLangCode(), kv.Key.Var, kv.Value);
					VPT.VPTItem item = new VPT.VPTItem();
					item.Name = kv.Key.Name;
					item.Cards.Add(card);
					sectionM.Items.Add(item);
				}

				VPT.VPTSection sectionS = new VPT.VPTSection();
				sectionS.ID = "sideboard";
				foreach (KeyValuePair<Card, int> kv in deck.SideBoard)
				{
					VPT.VPTCard card = new VPT.VPTCard(kv.Key.SetCode, "ENG", kv.Key.Var, kv.Value);
					VPT.VPTItem item = new VPT.VPTItem();
					item.Name = kv.Key.Name;
					item.Cards.Add(card);
					sectionS.Items.Add(item);
				}

				VPT.VPTDeck vdeck = new VPT.VPTDeck("mtg", deck.Mode.ToString(), deck.Format.ToString(), deck.Name, new List<VPT.VPTSection>()
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

		private MWS.MWSDeck ConvertToMDeck(Deck deck)
		{
			try
			{
				MWS.MWSDeck mdeck = new MWS.MWSDeck();
				foreach (KeyValuePair<Card, int> kv in deck.MainBoard)
				{
					MWS.MWSCard mcard = new MWS.MWSCard();
					mcard.Name = kv.Key.Name;
					mcard.SetCode = kv.Key.SetCode;
					mcard.Var = kv.Key.Var;
					mcard.Count = kv.Value;

					if (kv.Key.TypeCode.Contains("L"))
					{
						mdeck.MainBoardLands.Add(mcard);
					}
					else
					{
						mdeck.MainBoardSpells.Add(mcard);
					}
				}

				foreach (KeyValuePair<Card, int> kv in deck.SideBoard)
				{
					MWS.MWSCard mcard = new MWS.MWSCard();
					mcard.Name = kv.Key.Name;
					mcard.SetCode = kv.Key.SetCode;
					mcard.Var = kv.Key.Var;
					mcard.Count = kv.Value;

					mdeck.SideBoard.Add(mcard);
				}

				mdeck.Name = deck.Name;
				mdeck.Comment = deck.Comment;

				return mdeck;
			}
			catch
			{
				throw;
			}
		}

		private MAGE.MAGEDeck ConvertToGDeck(Deck deck)
		{
			try
			{
				MAGE.MAGEDeck gdeck = new MAGE.MAGEDeck();
				foreach (KeyValuePair<Card, int> kv in deck.MainBoard)
				{
					MAGE.MAGECard gcard = new MAGE.MAGECard();
					gcard.Name = kv.Key.Name;
					gcard.SetCode = kv.Key.SetCode;
					gcard.Number = kv.Key.Number;
					gcard.Count = kv.Value;

					gdeck.MainBoard.Add(gcard);
				}

				foreach (KeyValuePair<Card, int> kv in deck.SideBoard)
				{
					MAGE.MAGECard gcard = new MAGE.MAGECard();
					gcard.Name = kv.Key.Name;
					gcard.SetCode = kv.Key.SetCode;
					gcard.Number = kv.Key.Number;
					gcard.Count = kv.Value;

					gdeck.SideBoard.Add(gcard);
				}

				gdeck.Name = deck.Name;

				return gdeck;
			}
			catch
			{
				throw;
			}
		}

		private MO.MODeck ConvertToODeck(Deck deck)
		{
			try
			{
				MO.MODeck odeck = new MO.MODeck();
				foreach (KeyValuePair<Card, int> kv in deck.MainBoard)
				{
					MO.MOCard ocard = new MO.MOCard();
					ocard.Name = kv.Key.Name;
					ocard.Count = kv.Value;
					odeck.MainBoard.Add(ocard);
				}

				foreach (KeyValuePair<Card, int> kv in deck.SideBoard)
				{
					MO.MOCard ocard = new MO.MOCard();
					ocard.Name = kv.Key.Name;
					ocard.Count = kv.Value;
					odeck.SideBoard.Add(ocard);
				}
				odeck.Name = deck.Name;

				return odeck;
			}
			catch
			{
				throw;
			}
		}

		private Hyper.HyperDeck ConvertToXDeck(Deck deck)
		{
			try
			{
				var main = deck.MainBoard.Select(c => String.Format("{0}#{1}", c.Key.ID, c.Value));
				var side = deck.SideBoard.Select(c => String.Format("{0}#{1}", c.Key.ID, c.Value));
				Hyper.HyperDeck xdeck = new Hyper.HyperDeck(deck.Name, deck.Comment, deck.Format, deck.Mode, main, side);

				return xdeck;
			}
			catch
			{
				throw;
			}
		}

	}
}
