using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyperCore.Common;
using System.Reflection;

namespace HyperCore.IO
{
	public class ImageHandler
	{

		private ZipComp zipComp;

		/// <summary>
		/// Initializes a new instance of the Images class.
		/// </summary>
		public ImageHandler(string srcPath, string tmpPath)
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
		/// <param name="args"></param>
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
	}
}
