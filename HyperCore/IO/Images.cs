using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperCore.Common;

namespace HyperCore.IO
{
	public class Images
	{

		private ZipComp zipComp;

		/// <summary>
		/// Initializes a new instance of the Images class.
		/// </summary>
		public Images(string srcPath, string tmpPath)
		{
			zipComp = new ZipComp(srcPath, tmpPath);
		}

		/// <summary>
		/// Download image
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
		/// Download images
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
	}
}
