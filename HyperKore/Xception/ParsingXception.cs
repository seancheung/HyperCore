using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperKore.Xception
{
	public class ParsingXception : HyperXception
	{
		private Common.Card card;
		private string p;
		private Exception ex;

		public ParsingXception(Common.Card card, string p, Exception ex)
		{
			// TODO: Complete member initialization
			this.card = card;
			this.p = p;
			this.ex = ex;
		}
	}
}
