using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperCore.Utilities
{
	public static class Spliter
	{
		/// <summary>
		/// Split Setname 'Abcdef(ABC) --> Abcdef + ABC'
		/// </summary>
		/// <param name="setname">String to split, in the format of 'Abcdef(ABC)'</param>
		/// <returns>Two split strings</returns>
		public static string[] SplitSetName(this string setname)
		{
			return setname.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
