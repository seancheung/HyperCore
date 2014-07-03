using HyperCore.Common;
using System;
using System.Collections.Generic;

namespace HyperCore.Utilities
{
	public static class CardTool
	{
		/// <summary>
		/// Whether this card is doublefaced
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static bool IsDoubleFaced(this Card card)
		{
			return card.ID.Contains("|");
		}

		/// <summary>
		/// Whether this card is split
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static bool IsSplit(this Card card)
		{
			return card.Cost.Contains("|") && !card.ID.Contains("|");
		}

		/// <summary>
		/// Get split IDs
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetIDs(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.ID;
			else
			{
				foreach (var id in card.ID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return id;
			}
		}

		/// <summary>
		/// Get split zIDs
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetzIDs(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.zID;
			else
			{
				foreach (var id in card.zID.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return id;
			}
		}

		/// <summary>
		/// Get split Names
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetNames(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Name;
			else
			{
				foreach (var name in card.Name.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return name;
			}
		}

		/// <summary>
		/// Get split zNames
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetzNames(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.zName;
			else
			{
				foreach (var name in card.zName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return name;
			}
		}

		/// <summary>
		/// Get split Costs
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCosts(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Cost;
			else
			{
				foreach (var cost in card.Cost.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return cost;
			}
		}

		/// <summary>
		/// Get split Types
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetTypes(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Type;
			else
			{
				foreach (var type in card.Type.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return type;
			}
		}

		/// <summary>
		/// Get split zTypes
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetzTypes(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.zType;
			else
			{
				foreach (var type in card.zType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return type;
			}
		}

		/// <summary>
		/// Get split Powers
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetPows(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Pow;
			else
			{
				foreach (var pow in card.Pow.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return pow;
			}
		}

		/// <summary>
		/// Get split Toughnesses
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetTghs(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Tgh;
			else
			{
				foreach (var tgh in card.Tgh.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return tgh;
			}
		}

		/// <summary>
		/// Get split Texts
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetTexts(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Text;
			else
			{
				foreach (var text in card.Text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return text;
			}
		}

		/// <summary>
		/// Get split zTexts
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetzTexts(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.zText;
			else
			{
				foreach (var text in card.zText.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return text;
			}
		}

		/// <summary>
		/// Get split Flavors
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetFlavors(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Flavor;
			else
			{
				foreach (var flavor in card.Flavor.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return flavor;
			}
		}

		/// <summary>
		/// Get split zFlavors
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetzFlavors(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.zFlavor;
			else
			{
				foreach (var flavor in card.zFlavor.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return flavor;
			}
		}

		/// <summary>
		/// Get split Numbers
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetNumbers(this Card card)
		{
			if (!card.IsDoubleFaced())
				yield return card.Number;
			else
			{
				foreach (var number in card.Number.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
					yield return number;
			}
		}

		/// <summary>
		/// Get split Vars
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetVars(this Card card)
		{
			if (!card.Var.Contains(":"))
				yield return card.Var;
			else
			{
				foreach (var item in card.Var.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries))
					yield return item.Substring(2);
			}
		}

		/// <summary>
		/// Get split names
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		public static string GetLegalName(this Card card)
		{
			if (card.IsDoubleFaced())
			{
				return new List<string>(card.GetNames())[0];
			}
			else
			{
				return card.Name;
			}
		}

		/// <summary>
		/// Copy a card's properties' values from a target card
		/// </summary>
		/// <param name="card"></param>
		/// <param name="target"></param>
		public static void CopyFrom(this Card card, Card target)
		{
			if (card == null || target == null || card == target)
			{
				return;
			}

			foreach (var p in typeof(Card).GetProperties())
			{
				//Make sure it's readable
				if (p.CanWrite)
					p.SetValue(card, typeof(Card).GetProperty(p.Name).GetValue(target, null), null);
			}
		}
	}
}
