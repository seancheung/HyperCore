using System;

namespace HyperCore.Exceptions
{
	public class RequestException : HyperException
	{
		public int TriedTimes
		{
			get;
			private set;
		}
		public string URL
		{
			get;
			protected set;
		}

		/// <summary>
		/// Initializes a new instance of the RequestException
		/// </summary>
		public RequestException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the RequestException with parameters
		/// </summary>
		/// <param name="triedTimes"></param>
		/// <param name="url"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public RequestException(int triedTimes, string url, string message, Exception inner)
		: base("{0}\nTriedTimes: {1}\nURL: {2}", inner, message, triedTimes, url)
		{
			TriedTimes = triedTimes;
			URL = url;
		}
	}
}
