using System;
using System.Runtime.Serialization;

namespace HyperCore.Exceptions
{
	public class HyperException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the HyperException class
		/// </summary>
		public HyperException()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the HyperException class with parameters
		/// </summary>
		/// <param name="message">Exception message</param>
		public HyperException(string message)
			: base(message)
		{

		}

		/// <summary>
		/// Initializes a new instance of the HyperException class with parameters
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="inner">Inner exception</param>
		public HyperException(string message, Exception inner)
			: base(message, inner)
		{

		}

		/// <summary>
		/// Initializes a new instance of the HyperException class with parameters
		/// </summary>
		/// <param name="format"></param>
		/// <param name="inner"></param>
		/// <param name="args"></param>
		public HyperException(string format, Exception inner, params object[] args)
			: base(string.Format(format, args), inner)
		{

		}
	}
}
