using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperCore.Exceptions
{
	public class CardMissingException : HyperException
	{
		public string Name { get; set; }
		public string SetCode { get; set; }
		public string Set { get; set; }
		public string Number { get; set; }

		/// <summary>
		/// Initializes a new instance of the CardMissingException class.
		/// </summary>
		public CardMissingException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the CardMissingException class.
		/// </summary>
		public CardMissingException(string message, Exception inner, string name, string setCode)
			: base("{0}\n{1} - {2}", inner, message, name, setCode)
		{
			Name = name;
			SetCode = setCode;
		}

		/// <summary>
		/// Initializes a new instance of the CardMissingException class.
		/// </summary>
		public CardMissingException(string message, Exception inner, string @set, string number,string setcode = "")
			: base("{0}\n{1} - {2}", inner, message, set, number)
		{
			Set = @set;
			Number = number;
			SetCode = setcode;
		}
	}
}
