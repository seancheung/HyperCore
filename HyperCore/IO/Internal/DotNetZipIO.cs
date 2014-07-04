using HyperCore.Common;
using HyperCore.Data;
using HyperCore.Utilities;
using Ionic.Zip;
using System;
using System.IO;

namespace HyperCore.IO
{
	internal class DotNetZipIO
	{
		/// <summary>
		/// Zip process locker
		/// </summary>
		private static object _lock = new object();

		/// <summary>
		/// Image folder path
		/// </summary>
		public string SrcPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Temp files path
		/// </summary>
		public string TmpPath
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializer
		/// </summary>
		/// <param name="srcPath">Image folder path</param>
		/// <param name="tmpPath">Temp files path</param>
		public DotNetZipIO(string srcPath, string tmpPath)
		{
			SrcPath = srcPath;
			TmpPath = tmpPath;
		}

		/// <summary>
		/// Pack card image(s)
		/// </summary>
		/// <param name="card"></param>
		public void Pack(Card card)
		{
			foreach (var id in card.GetIDs())
			{
				Pack(card.SetCode, id);
			}

			foreach (var id in card.GetzIDs())
			{
				Pack(card.SetCode, id);
			}

		}

		/// <summary>
		/// Unpack card image(s)
		/// </summary>
		/// <param name="card"></param>
		public void UnPack(Card card)
		{
			foreach (var id in card.GetIDs())
			{
				UnPack(card.SetCode, id);
			}

			foreach (var id in card.GetzIDs())
			{
				UnPack(card.SetCode, id);
			}
		}

		/// <summary>
		/// Pack image by id
		/// </summary>
		/// <param name="setCode"></param>
		/// <param name="id"></param>
		public void Pack(string setCode, string id)
		{
			if (!Directory.Exists(TmpPath))
			{
				Directory.CreateDirectory(TmpPath);
			}
			if (!Directory.Exists(SrcPath))
			{
				Directory.CreateDirectory(SrcPath);
			}

			string zipPath = string.Format("{0}{1}.zip", SrcPath, setCode);

			if (!File.Exists(zipPath))
			{
				using (ZipFile zipFile = new ZipFile(zipPath))
				{
					zipFile.Save();
				}
			}

			using (ZipFile zipFile = ZipFile.Read(zipPath))
			{
				if (!zipFile.ContainsEntry(string.Format("{0}.jpg", id)))
				{
					try
					{
						lock (_lock)
						{
							DownloadImage.Instance.DownloadBytes(id, TmpPath);
							zipFile.AddFile(String.Format("{0}{1}.jpg", TmpPath, id), "\\");
							zipFile.Save();
						}
					}
					catch (Exception ex)
					{

						throw;
					}
				}
			}
		}

		/// <summary>
		/// Unpack image by id
		/// </summary>
		/// <param name="setCode"></param>
		/// <param name="id"></param>
		public void UnPack(string setCode, string id)
		{
			if (!Directory.Exists(TmpPath))
			{
				Directory.CreateDirectory(TmpPath);
			}
			if (!Directory.Exists(SrcPath))
			{
				Directory.CreateDirectory(SrcPath);
			}

			string zipPath = string.Format("{0}{1}.zip", SrcPath, setCode);

			if (!File.Exists(zipPath))
			{
				using (ZipFile zipFile = new ZipFile(zipPath))
				{
					zipFile.Save();
				}
			}

			using (ZipFile zipFile = ZipFile.Read(zipPath))
			{
				if (!zipFile.ContainsEntry(string.Format("{0}.jpg", id)))
				{
					Pack(id, setCode);
				}

				try
				{
					lock (_lock)
					{
						zipFile[id + ".jpg"].Extract(TmpPath, ExtractExistingFileAction.DoNotOverwrite);
					}
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		public void Wrap(string zipName, string id, string newName)
		{
			if (!Directory.Exists(TmpPath))
			{
				Directory.CreateDirectory(TmpPath);
			}
			if (!Directory.Exists(SrcPath))
			{
				Directory.CreateDirectory(SrcPath);
			}

			string zipPath = string.Format("{0}{1}.zip", SrcPath, zipName);

			if (!File.Exists(zipPath))
			{
				using (ZipFile zipFile = new ZipFile(zipPath))
				{
					zipFile.Save();
				}
			}

			using (ZipFile zipFile = ZipFile.Read(zipPath))
			{
				if (!zipFile.ContainsEntry(string.Format("{0}.jpg", newName)))
				{
					try
					{
						lock (_lock)
						{
							File.Move(String.Format("{0}{1}.jpg", TmpPath, id), String.Format("{0}{1}.jpg", TmpPath, newName));
							zipFile.AddFile(String.Format("{0}{1}.jpg", TmpPath, newName), "\\");
							zipFile.Save();
						}
					}
					catch (Exception ex)
					{

						throw;
					}
				}
			}
		}
	}
}
