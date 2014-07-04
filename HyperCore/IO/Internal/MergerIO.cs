using HyperCore.Exceptions;
using System;
using System.IO;

namespace HyperCore.IO
{
	internal class MergerIO
	{
		/// <summary>
		/// Merge multiple files
		/// </summary>
		/// <param name="inputFiles">Full paths of files to merge</param>
		/// <param name="outputFile">Output file full path</param>
		public void Merge(string[] inputFiles, string outputFile)
		{
			using (var fs = File.Create(outputFile))
			{
				using (var bw = new BinaryWriter(fs))
				{
					foreach (var file in inputFiles)
					{
						try
						{
							//Mark file breaking point
							bw.Write(true);
							//Mark file legal name
							bw.Write(Path.GetFileName(file));
							//Size length of the file
							var data = File.ReadAllBytes(file);
							//Track length
							bw.Write(data.Length);
							//Save file data
							bw.Write(data);
						}
						catch (Exception ex)
						{
							throw new IOFileException(file, outputFile, "IO Error happended when merging files", ex);
						}
					}
					//Mark end
					bw.Write(false);
				}
			}
		}

		/// <summary>
		/// Split File to multiple
		/// </summary>
		/// <param name="inputFile">File to split</param>
		/// <param name="outputPath">Path to split to</param>
		public void Split(string inputFile, string outputPath)
		{
			using (var br = new BinaryReader(File.OpenRead(inputFile)))
			{
				while (br.ReadBoolean())
				{
					try
					{
						using (var fs = File.Create(string.Format(@"{0}\{1}", outputPath, br.ReadString())))
						{
							using (var bw = new BinaryWriter(fs))
							{
								var len = br.ReadInt32();
								var data = br.ReadBytes(len);
								bw.Write(data);
							}
						}
					}
					catch (Exception ex)
					{
						throw new IOFileException(inputFile, outputPath, "IO Error happended when splitting file", ex);
					}

				}
			}
		}
	}
}
