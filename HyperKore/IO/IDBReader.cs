using HyperKore.Common;
using System.Collections.Generic;

namespace HyperKore.IO
{
	public interface IDBReader
	{
		/// <summary>
		/// Load cards
		/// </summary>
		/// <returns></returns>
		IEnumerable<Card> LoadCards();

		/// <summary>
		/// Load file
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		byte[] LoadFile(string id, ICompressor compressor);

		/// <summary>
		/// Load sets
		/// </summary>
		/// <returns></returns>
		IEnumerable<Set> LoadSets();
	}
}