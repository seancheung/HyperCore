using System.IO;

namespace HyperKore.IO
{
	public interface IDeckWriter
	{
		public string Ext;
		public string Name;

		bool Write(Common.Deck deck, Stream output);
	}
}