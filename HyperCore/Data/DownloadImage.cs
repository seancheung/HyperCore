using HyperCore.Common;
using HyperCore.Net;
using HyperCore.Utilities;
using System;
using System.IO;
using System.Linq;

namespace HyperCore.Data
{
	public class DownloadImage
	{
		/// <summary>
		/// Single Instance
		/// </summary>
		public static readonly DownloadImage Instance = new DownloadImage();

		/// <summary>
		/// DownloadBytes card image
		/// </summary>
		/// <param name="card"></param>
		/// <param name="tmpPath"></param>
		/// <param name="lang"></param>
		/// <param name="site"></param>
		public void DownloadBytes(Card card, string tmpPath, LANGUAGE lang = LANGUAGE.English, WEBSITE site = WEBSITE.gatherer)
		{
			if (!Directory.Exists(tmpPath))
			{
				Directory.CreateDirectory(tmpPath);
			}
			string[] ids = lang == LANGUAGE.English || string.IsNullOrWhiteSpace(card.zID) ?
						   card.GetIDs().ToArray() :
						   card.GetzIDs().ToArray();
			string[] nums = card.GetNumbers().ToArray();

			for (int i = 0; i < Math.Min(ids.Length, nums.Length); i++)
			{
				if (!File.Exists(string.Format("{0}{1}.jpg", tmpPath, ids[i])) || new FileInfo(string.Format("{0}{1}.jpg", tmpPath, ids[i])).Length == 0L)
				{
					string url = GetURL(ids[i], card.SetCode, nums[i], lang, site);
					try
					{
						Downloader.Instance.Downloadfile(url, string.Format("{0}{1}.jpg", tmpPath, ids[i]));
					}
					catch
					{
						throw;
					}
				}
			}
		}

		/// <summary>
		/// DownloadBytes card image
		/// </summary>
		/// <param name="id"></param>
		/// <param name="tmpPath"></param>
		public void DownloadBytes(string id, string tmpPath)
		{
			if (!File.Exists(string.Format("{0}{1}.jpg", tmpPath, id)) || new FileInfo(string.Format("{0}{1}.jpg", tmpPath, id)).Length == 0L)
			{
				string url = GetURL(id, null, null);
				try
				{
					Downloader.Instance.Downloadfile(url, string.Format("{0}{1}.jpg", tmpPath, id));
				}
				catch
				{
					throw;
				}
			}

		}

		/// <summary>
		/// Dwonload file into byte array
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public byte[] DownloadBytes(string id)
		{
			string url = GetURL(id, null, null);
			try
			{
				return Downloader.Instance.DownloadByte(url);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Get image downloading url
		/// </summary>
		/// <param name="id"></param>
		/// <param name="setcode"></param>
		/// <param name="num"></param>
		/// <param name="lang"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		private string GetURL(string id, string setcode, string num, LANGUAGE lang = LANGUAGE.English, WEBSITE site = WEBSITE.gatherer)
		{
			//Default is gatherer
			string result = string.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", id);

			switch (site)
			{
				case WEBSITE.gatherer:
					break;
				case WEBSITE.magiccards:
					result = string.Format("http://magiccards.info/scans/{0}/{1}/{2}.jpg", lang.GetLangCode(), setcode.ToLower(), num);
					break;
				case WEBSITE.magicspoiler:
					break;
				case WEBSITE.iplaymtg:
					result = string.Format("http://data.iplaymtg.com/mtgdeck/card/{0}/{1}/{2}.jpg", lang.GetLangCode(), setcode.ToUpper(), num);
					break;
				default:
					break;
			}

			return result;

		}
	}
}
