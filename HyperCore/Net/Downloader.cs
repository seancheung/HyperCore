using HyperCore.Exceptions;
using System;
using System.Net;

namespace HyperCore.Net
{
	public class Downloader
	{
		private Downloader() { }

		/// <summary>
		/// Downlaod file from the provided url and store it to the provided path
		/// </summary>
		/// <param name="url">Download link</param>
		/// <param name="path">Stroring path</param>
		public static void Downloadfile(string url, string path)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers.Add("User_Agent", "Chrome");
					webClient.DownloadFile(url, path);
				}
			}
			catch (Exception ex)
			{
				throw new DownloaderException(url, path, "Downloading File Error", ex);
			}
		}
	}
}
