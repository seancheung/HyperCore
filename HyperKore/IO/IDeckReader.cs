using System.IO;

namespace HyperKore.IO
{
	public interface IDeckReader
	{
		public string Ext;
		public string Name;

		Common.Deck Read(Stream input);
	}
}