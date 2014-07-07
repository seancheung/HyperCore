using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperKore.Web
{
	public interface ICardParse
	{
		/// <summary>
		/// Parse the card
		/// </summary>
		/// <param name="card"></param>
		void Parse(Common.Card card, Common.LANGUAGE lang);
	}
}
