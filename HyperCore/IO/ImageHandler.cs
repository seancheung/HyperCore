using HyperCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperCore.Utilities;
using HyperCore.Data;
using System.IO;

namespace HyperCore.IO
{
	public class ImageHandler
	{

		private DotNetZipIO zipComp;

		/// <summary>
		/// Initializes a new instance of the Images class.
		/// </summary>
		public ImageHandler(string srcPath, string tmpPath)
		{
			zipComp = new DotNetZipIO(srcPath, tmpPath);
		}

		/// <summary>
		/// DownloadBytes image
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public bool DownLoad(Card card)
		{
			try
			{
				zipComp.Pack(card);
				return true;
			}
			catch (Exception ex)
			{
				//do sth here
				return false;
			}
		}

		/// <summary>
		/// DownloadBytes images
		/// </summary>
		/// <param name="cards"></param>
		/// <returns>Failed cards</returns>
		public IEnumerable<Card> DownLoad(IEnumerable<Card> cards)
		{
			foreach (var card in cards)
			{
				try
				{
					zipComp.Pack(card);
					continue;
				}
				catch (Exception ex)
				{
					//do sth here
				}
				yield return card;
			}
		}

		/// <summary>
		/// Extract an image
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public bool Extract(Card card)
		{
			try
			{
				zipComp.UnPack(card);
				return true;
			}
			catch (Exception ex)
			{
				//do sth here
				return false;
			}
		}

		/// <summary>
		/// Rename a card by custom expression
		/// </summary>
		/// <param name="card"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public bool Rename(Card card, params string[] args)
		{
			if (!Extract(card))
			{
				return false;
			}

			StringBuilder sb = new StringBuilder();

			var props = typeof(Card).GetProperties();
			foreach (var arg in args)
			{
				var prop = props.FirstOrDefault(p => p.Name.Equals(arg, StringComparison.OrdinalIgnoreCase));
				if (prop != null)
				{
					sb.Append(
					    prop.GetValue(card, null) ?? arg
					);
				}
				else
				{
					sb.Append(arg);
				}

			}

			try
			{
				zipComp.Wrap(card.Set, card.ID, sb.ToString());
				return true;
			}
			catch (Exception ex)
			{
				//do sth
				return false;
			}

		}

		/// <summary>
		/// Rename cards by custom expression
		/// </summary>
		/// <param name="cards"></param>
		/// <param name="args">Rename expression, like 'setcode - id(number).name'</param>
		/// <returns>Failed cards</returns>
		public IEnumerable<Card> Rename(IEnumerable<Card> cards, params string[] args)
		{
			foreach (var card in cards)
			{
				if (!Rename(card, args))
				{
					yield return card;
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
					var data = DownloadImage.Instance.DownloadBytes(id);
					SaveImage(id, data);
				}

				foreach (string id in card.GetzIDs())
				{
					var data = DownloadImage.Instance.DownloadBytes(id);
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
		/// Read card images to provided path
		/// </summary>
		/// <param name="card"></param>
		/// <param name="path"></param>
		public void ReadImages(Card card, string path)
		{
			SQLiteIO sql = new SQLiteIO();
			foreach (var id in card.GetIDs())
			{
				var bytes = sql.LoadFile(id);
				if (bytes != null)
				{
					File.WriteAllBytes(String.Format("{0}/{1}.jpg", path, id), bytes);
				}
				
			}
		}

		/// <summary>
		/// Read Read card images to streams
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public IEnumerable<Stream> ReadImages(Card card)
		{
			SQLiteIO sql = new SQLiteIO();
			foreach (var id in card.GetIDs())
			{
				var bytes = sql.LoadFile(id);
				if (bytes == null)
				{
					yield break;
				}
				
				yield return new MemoryStream(bytes);
			}
		}
	}
}
