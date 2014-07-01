using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace HyperCore.IO
{
	internal class GZipComp
	{
		/// <summary>
		/// Compress target ByteArray
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public byte[] Compress(byte[] data)
		{
			MemoryStream ms = new MemoryStream();
			GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress);
			zipStream.Write(data, 0, data.Length);
			zipStream.Close();
			return ms.ToArray();
		}

		/// <summary>
		/// Decompress target ByteArray with original length
		/// </summary>
		/// <param name="data"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public byte[] Decompress(byte[] data, int length)
		{
			MemoryStream srcMs = new MemoryStream(data);
			GZipStream zipStream = new GZipStream(srcMs, CompressionMode.Decompress);
			MemoryStream ms = new MemoryStream();
			byte[] bytes = new byte[length];
			int n;
			while ((n = zipStream.Read(bytes, 0, bytes.Length)) > 0)
			{
				ms.Write(bytes, 0, n);
			}
			zipStream.Close();
			return ms.ToArray();
		}

		/// <summary>
		/// Decompress target ByteArray with default length of 40960
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public byte[] Decompress(byte[] data)
		{
			return Decompress(data, 40960);
		}
	}
}
