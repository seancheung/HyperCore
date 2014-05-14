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
			case FILETYPE.Virtual_Play_Table:
				break;
			case FILETYPE.Magic_Workstation:
				ext = "mwDeck";
				break;
			case FILETYPE.Mage:
			case FILETYPE.Magic_Online:
				ext = "txt";
				break;
			default:
				break;
			}

			return ext;
		}
	}
}
