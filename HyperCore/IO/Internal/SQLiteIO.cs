using HyperCore.Common;
using HyperCore.Utilities;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;

namespace HyperCore.IO
{
	internal class SQLiteIO
	{
		/// <summary>
		/// Single instance of SQLiteConnection
		/// </summary>
		private static readonly SQLiteConnection conn = new SQLiteConnection();

		/// <summary>
		/// Initializes a new instance of the SQLiteIO class.
		/// </summary>
		public SQLiteIO()
		{
			conn.ConnectionString = Resource.ConnectionCommandSQLite;
			Create();

		}

		/// <summary>
		/// CreateXmlData Database and Table
		/// </summary>
		private void Create()
		{
			using (var datacontext = new DataContext(conn))
			{
				datacontext.ExecuteCommand(Resource.BuildCommand);
			}
		}

		/// <summary>
		/// Add a card
		/// </summary>
		/// <param name="card"></param>
		public bool Insert(Card card)
		{

			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() != 0)
				{
					return Update(card);
				}

				tab.InsertOnSubmit(card);
				datacontext.SubmitChanges();
				return true;
			}

		}

		/// <summary>
		/// Add legalities
		/// </summary>
		/// <param name="sets"></param>
		public void Insert(IEnumerable<SetInfo> sets)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<SetInfo>();
				tab.InsertAllOnSubmit(sets);
				datacontext.SubmitChanges();
			}
		}

		/// <summary>
		/// Add cards
		/// </summary>
		/// <param name="cards"></param>
		public void Insert(IEnumerable<Card> cards)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				foreach (var card in cards)
				{
					Insert(card);
				}
			}
		}

		/// <summary>
		/// Add a file as byteArray
		/// </summary>
		/// <param name="id"></param>
		/// <param name="data"></param>
		public bool Insert(string id, byte[] data)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<FileData>();
				var compdata = new GZipIO().Compress(data);
				var que = tab.Where(i => i.ID == id);

				if (que.Count() != 0)
				{
					return Update(id, data);
				}

				tab.InsertOnSubmit(new FileData()
				{
					ID = id,
					Data = compdata,
					Length = data.Length
				});
				datacontext.SubmitChanges();
				return true;
			}
		}

		/// <summary>
		/// Delete a card
		/// </summary>
		/// <param name="card"></param>
		public bool Delete(Card card)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() == 0)
				{
					return false;
				}

				tab.DeleteAllOnSubmit(que);
				datacontext.SubmitChanges();

				return true;
			}
		}

		/// <summary>
		/// Delete file(s) with provided id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(string id)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<FileData>();
				var que = tab.Where(i => i.ID == id);

				if (que.Count() == 0)
				{
					return false;
				}

				tab.DeleteAllOnSubmit(que);
				datacontext.SubmitChanges();

				return true;
			}
		}

		/// <summary>
		/// Update a card
		/// </summary>
		/// <param name="card"></param>
		public bool Update(Card card)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				var que = tab.Where(c => c.ID == card.ID);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.CopyFrom(card);
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		/// <summary>
		/// Update file data
		/// </summary>
		/// <param name="id"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool Update(string id, byte[] data)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<FileData>();
				var compdata = new GZipIO().Compress(data);
				var que = tab.Where(i => i.ID == id);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.Data = compdata;
					item.Length = data.Length;
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		/// <summary>
		/// Update setinfo
		/// </summary>
		/// <param name="setinfo"></param>
		/// <returns></returns>
		public bool Update(SetInfo setinfo)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<SetInfo>();
				var que = tab.Where(c => c.SetName == setinfo.SetName);

				if (que.Count() == 0)
				{
					return false;
				}

				foreach (var item in que)
				{
					item.SetCode = setinfo.SetCode;
					item.LastUpdate = setinfo.LastUpdate;
					item.Local = setinfo.Local;
				}
				datacontext.SubmitChanges();
				return true;
			}
		}

		/// <summary>
		/// Load card database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Card> LoadCard()
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<Card>();
				return tab.ToList();
			}
		}

		/// <summary>
		/// Load setinfo database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SetInfo> LoadSetinfo()
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<SetInfo>();
				return tab.ToList();
			}
		}

		/// <summary>
		/// Load file byte array
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public byte[] LoadFile(string id)
		{
			using (var datacontext = new DataContext(conn))
			{
				var tab = datacontext.GetTable<FileData>();
				var datas = tab.Where(i => i.ID == id).ToArray();

				if (datas.Count() != 1)
				{
					return null;
				}

				return new GZipIO().Decompress(datas[0].Data, datas[0].Length);
			}
		}

	}
}
