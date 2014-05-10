using HyperCore.Exceptions;
using System;
using System.IO;
using System.Net;

namespace HyperCore.Net
{
	public class Request
	{
		private Request() { }

		/// <summary>
		/// Get Data from the provided url
		/// </summary>
		/// <param name="url">A url to create request</param>
		/// <returns>Data from the response</returns>
		public static string GetWebData(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.AllowAutoRedirect = false;
			string data = string.Empty;

			//Max try times
			int tryCount = 10;

			while (tryCount > 0)
			{
				try
				{
					using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
					{
						if (!httpWebResponse.StatusDescription.Equals("Found"))
						{
							StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
							data = streamReader.ReadToEnd();
						}
					}
					break;
				}
				catch (Exception ex)
				{
					//If time-out, retry
					if (ex is WebException && (ex as WebException).Status == WebExceptionStatus.Timeout)
					{
						tryCount--;
					}
					else
					{
						throw new RequestException(tryCount, url, "Requesting Error", ex);
					}
				}
			}

			return data;

		}
	}
}
