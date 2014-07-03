using HyperCore.Common;
using HyperCore.Data;
using HyperCore.Exceptions;
using HyperCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HyperCore.IO
{
	public class Database
	{
		/// <summary>
		/// Class for Serialization
		/// </summary>
		[XmlRoot("database")]
		public class XmlData
		{
			[XmlAttribute("update")]
			public string Date
			{
				get;
				set;
			}

			[XmlArray("cards"), XmlArrayItem("card")]
			public List<Card> Cards
			{
				get;
				set;
			}

			[XmlArray("formats"), XmlArrayItem("format")]
			public List<Format> Formats
			{
				get;
				set;
			}

			[XmlArray("sets"), XmlArrayItem("set")]
			public List<string> Sets
			{
				get;
				set;
			}

			public XmlData()
			{
				Date = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss");
				Cards = new List<Card>();
				Formats = new List<Format>();
				Sets = new List<string>();
			}

			public XmlData(IEnumerable<Card> cards)
			{
				Date = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss");
				Cards = new List<Card>(cards);
				Formats = new List<Format>();
				Sets = new List<string>();
			}

			public XmlData(IEnumerable<Format> formats)
			{
				Date = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss");
				Formats = new List<Format>(formats);
				Cards = new List<Card>();
				Sets = new List<string>();
			}

			public XmlData(IEnumerable<string> sets)
			{
				Date = DateTime.Now.ToString("yyyy-MM-dd/HH:mm:ss");
				Sets = new List<string>(sets);
				Cards = new List<Card>();
				Formats = new List<Format>();
			}
		}

		private Database() { }

		/// <summary>
		/// Single Instance
		/// </summary>
		public static readonly Database Instance = new Database();

		/// <summary>
		/// Load cards
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <returns></returns>
		public IEnumerable<Card> LoadCards(string xmlPath)
		{
			try
			{
				using (var stream = new FileStream(xmlPath, FileMode.Open))
				{
					var serializer = new XmlSerializer(typeof(XmlData));
					var data = serializer.Deserialize(stream) as XmlData;
					return data.Cards;

				}
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when loading card database", ex);
			}
		}

		/// <summary>
		/// Load cards form local db
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Card> LoadCards()
		{
			try
			{
				SQLiteIO sql = new SQLiteIO();
				return sql.LoadCard();
			}
			catch (Exception ex)
			{
				throw new IOFileException("IO Error happended when loading card database", ex);
			}
		}

		/// <summary>
		/// Load formats
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <returns></returns>
		public IEnumerable<Format> LoadFormats(string xmlPath)
		{
			try
			{
				using (var stream = new FileStream(xmlPath, FileMode.Open))
				{
					var serializer = new XmlSerializer(typeof(XmlData));
					var data = serializer.Deserialize(stream) as XmlData;
					return data.Formats;

				}
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when loading format database", ex);
			}
		}

		/// <summary>
		/// Load sets
		/// </summary>
		/// <param name="xmlPath"></param>
		/// <returns></returns>
		public IEnumerable<string> LoadSets(string xmlPath)
		{
			try
			{
				using (var stream = new FileStream(xmlPath, FileMode.Open))
				{
					var serializer = new XmlSerializer(typeof(XmlData));
					var data = serializer.Deserialize(stream) as XmlData;
					return data.Sets;

				}
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when loading set database", ex);
			}
		}

		/// <summary>
		/// Load sets form database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SetInfo> LoadSets()
		{
			try
			{
				SQLiteIO sql = new SQLiteIO();
				return sql.LoadSetinfo();
			}
			catch (Exception ex)
			{
				throw new IOFileException("IO Error happended when loading set database", ex);
			}
		}


		/// <summary>
		/// Create empty databse file
		/// </summary>
		/// <param name="xmlPath"></param>
		private void Create(string xmlPath)
		{
			try
			{

				using (var stream = new FileStream(xmlPath, FileMode.Create))
				{
					var serializer = new XmlSerializer(typeof(XmlData));
					var nas = new XmlSerializerNamespaces();
					nas.Add(string.Empty, string.Empty);
					serializer.Serialize(stream, new XmlData(), nas);
				}

			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when creating database", ex);
			}
		}

		/// <summary>
		/// Save cards
		/// </summary>
		/// <param name="cards"></param>
		/// <param name="xmlPath"></param>
		public void Save(IEnumerable<Card> cards, string xmlPath)
		{
			if (!File.Exists(xmlPath))
			{
				try
				{
					Create(xmlPath);
				}
				catch
				{
					throw;
				}
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(xmlPath);
				foreach (var card in cards)
				{
					var node = doc.SelectSingleNode(string.Format("/database/cards/card[@id='{0}']", card.ID));

					var xnode = SerializeNode(card, "card");

					if (node == null)
					{
						var parent = doc.SelectSingleNode("/database/cards");
						var newNode = doc.ImportNode(xnode, true);
						parent.AppendChild(newNode);
					}
					else
					{
						var newNode = doc.ImportNode(xnode, true);
						node.ParentNode.ReplaceChild(newNode, node);
					}

				}
				doc["database"].Attributes["update"].Value = new XmlData().Date;
				doc.Save(xmlPath);
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when saving cards database", ex);
			}
		}

		/// <summary>
		/// Save cards to local db
		/// </summary>
		/// <param name="cards"></param>
		public void Save(IEnumerable<Card> cards)
		{
			try
			{
				SQLiteIO sql = new SQLiteIO();
				sql.Insert(cards);
			}
			catch (Exception ex)
			{
				throw new IOFileException("IO Error happended when saving cards database", ex);
			}
		}

		/// <summary>
		/// Save format list
		/// </summary>
		/// <param name="formats"></param>
		/// <param name="xmlPath"></param>
		public void Save(IEnumerable<Format> formats, string xmlPath)
		{
			if (!File.Exists(xmlPath))
			{
				try
				{
					Create(xmlPath);
				}
				catch
				{
					throw;
				}
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(xmlPath);

				var xnode = SerializeNode(new XmlData(formats), "tmp").SelectSingleNode("/tmp/formats");
				var node = doc.SelectSingleNode("/database/formats");
				var newNode = doc.ImportNode(xnode, true);
				node.ParentNode.ReplaceChild(newNode, node);
				doc["database"].Attributes["update"].Value = new XmlData().Date;
				doc.Save(xmlPath);
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when saving formats database", ex);
			}
		}

		/// <summary>
		/// Save set list
		/// </summary>
		/// <param name="sets"></param>
		/// <param name="xmlPath"></param>
		public void Save(IEnumerable<string> sets, string xmlPath)
		{
			if (!File.Exists(xmlPath))
			{
				try
				{
					Create(xmlPath);
				}
				catch
				{
					throw;
				}
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(xmlPath);

				var xnode = SerializeNode(new XmlData(sets), "tmp").SelectSingleNode("/tmp/sets");
				var node = doc.SelectSingleNode("/database/sets");
				var newNode = doc.ImportNode(xnode, true);
				node.ParentNode.ReplaceChild(newNode, node);
				doc["database"].Attributes["update"].Value = new XmlData().Date;
				doc.Save(xmlPath);
			}
			catch (Exception ex)
			{
				throw new IOFileException(xmlPath, "IO Error happended when saving sets database", ex);
			}
		}

		/// <summary>
		/// Save set list
		/// </summary>
		/// <param name="sets"></param>
		public void Save(IEnumerable<string> sets)
		{
			var setinfo = new List<SetInfo>();

			try
			{
				foreach (var set in sets)
					{
						setinfo.Add(new SetInfo()
						{
							SetName = set.SplitSetName()[0],
							SetCode = set.SplitSetName()[1],
							LastUpdate = DateTime.MinValue,
							Local = false
						});
					}

				SQLiteIO sql = new SQLiteIO();
				sql.Insert(setinfo);
			}
			catch (Exception ex)
			{
				throw new IOFileException("IO Error happended when saving sets database", ex);
			}
		}

		/// <summary>
		/// Update setinfo
		/// </summary>
		/// <param name="set"></param>
		public void Update(string set)
		{
			var setinfo = new SetInfo()
			{
				SetName = set.SplitSetName()[0],
				SetCode = set.SplitSetName()[1],
				LastUpdate = DateTime.Now,
				Local = true
			};

			SQLiteIO sql = new SQLiteIO();
			sql.Update(setinfo);
		}

		/// <summary>
		/// Serialize object to xmlNode
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="rootName"></param>
		/// <returns></returns>
		private XmlNode SerializeNode(Object obj, string rootName)
		{
			using (var strw = new StringWriter())
			{
				try
				{
					var ser = new XmlSerializer(obj.GetType(), new XmlRootAttribute(rootName));
					var nas = new XmlSerializerNamespaces();
					nas.Add(string.Empty, string.Empty);
					ser.Serialize(strw, obj, nas);
					XmlDocument xdoc = new XmlDocument();
					xdoc.LoadXml(strw.ToString());
					return xdoc.DocumentElement;
				}
				catch
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Grab a card's images from online to local
		/// </summary>
		/// <param name="card"></param>
		public void GrabImage(Card card)
		{
			try
			{
				foreach (string id in card.GetIDs())
				{
					var data = DownloadImage.Instance.Download(id);
					SaveImage(id, data);
				}

				foreach (string id in card.GetzIDs())
				{
					var data = DownloadImage.Instance.Download(id);
					SaveImage(id, data);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Save byte array into database
		/// </summary>
		/// <param name="id"></param>
		/// <param name="data"></param>
		private bool SaveImage(string id, byte[] data)
		{
			SQLiteIO sql = new SQLiteIO();
			return sql.Insert(id, data);
		}

		/// <summary>
		/// Read card to provided path
		/// </summary>
		/// <param name="card"></param>
		/// <param name="path"></param>
		public void ReadImage(Card card, string path)
		{
			SQLiteIO sql = new SQLiteIO();
			foreach (var id in card.GetIDs())
			{
				var bytes = sql.LoadFile(id);
				File.WriteAllBytes(String.Format("{0}/{1}.jpg", path, id), bytes);
			}
		}

		//public void SaveAsJson(Card card, string filePath)
		//{
		//	using (var stream = File.Open(filePath, FileMode.Create))
		//	{
		//		var sw = new StreamWriter(stream);
		//		sw.Write(new JsonIO().Convert(card));
		//		sw.Flush();
		//	}
		//}

		//public Card ReadCardFromJson(string filePath)
		//{
		//	using (var stream = File.Open(filePath, FileMode.Open))
		//	{
		//		var sr = new StreamReader(stream);
		//		var json = sr.ReadToEnd();
		//		return new JsonIO().ReadCard(json);
		//	}
		//}
	}
}
