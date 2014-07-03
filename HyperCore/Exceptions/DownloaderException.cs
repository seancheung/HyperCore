using System;

namespace HyperCore.Exceptions
{
	public class DownloaderException : HyperException
	{
		public string URL
		{
			get;
			protected set;
		}
		public string Path
		{
			get;
			protected set;
		}

		/// <summary>
		/// Initializes a new instance of the DownloaderException
		/// </summary>
		public DownloaderException()
		{

		}

		public DownloaderException(string url, string message, Exception inner)
		: base("{0}\nURL: {1}", inner, message, url)
		{
			URL = url;
		}

		/// <summary>
		/// Initializes a new instance of the DownloaderException class with parameters
		/// </summary>
		/// <param name="url"></param>
		/// <param name="path"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public DownloaderException(string url, string path, string message, Exception inner)
		: base("{0}\nURL: {1}\nPath: {2}", inner, message, url, path)
		{
			URL = url;
			Path = path;
		}
	}
}
