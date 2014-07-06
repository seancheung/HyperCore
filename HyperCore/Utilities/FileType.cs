using HyperCore.Common;

namespace HyperCore.Utilities
{
	public static class FileType
	{
		/// <summary>
		/// Get filetype extesion
		/// </summary>
		/// <param name="ft"></param>
		/// <returns></returns>
		public static string GetFileExt(this FILETYPE ft)
		{
			string ext = "deck";
			switch (ft)
			{
				case FILETYPE.VirtualPlayTable:
					break;
				case FILETYPE.MagicWorkstation:
					ext = "mwDeck";
					break;
				case FILETYPE.HyperDeck:
					ext = "xDeck";
					break;
				case FILETYPE.Mage:
				case FILETYPE.MagicOnline:
					ext = "txt";
					break;
				default:
					break;
			}

			return ext;
		}
	}
}
