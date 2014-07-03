using HyperCore.Common;
using System;

namespace HyperCore.Exceptions
{
	public class ParseException : HyperException
	{
		public Card Card
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the ParseException class.
		/// </summary>
		public ParseException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the ParseException class.
		/// </summary>
		public ParseException(string message, Exception inner)
		: base(message, inner)
		{

		}

		/// <summary>
		/// Initializes a new instance of the ParseException class.
		/// </summary>
		public ParseException(Card card, string message, Exception inner)
		: base(message, inner)
		{
			Card = card;
		}
	}
}
