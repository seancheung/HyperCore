using HyperCore.Common;

namespace HyperCore.Utilities
{
	public static class Language
	{
		/// <summary>
		/// Get language code
		/// </summary>
		/// <param name="lang"></param>
		/// <returns></returns>
		public static string GetLangCode(this LANGUAGE lang)
		{
			string result = "en";

			switch (lang)
			{
			case LANGUAGE.Chinese_Simplified:
				result = "cn";
				break;
			case LANGUAGE.Chinese_Traditional:
				result = "tw";
				break;
			case LANGUAGE.German:
				result = "ge";
				break;
			case LANGUAGE.French:
				result = "fr";
				break;
			case LANGUAGE.Italian:
				result = "it";
				break;
			case LANGUAGE.Japanese:
				result = "jp";
				break;
			case LANGUAGE.Korean:
				result = "ko";
				break;
			case LANGUAGE.Portuguese:
				result = "pt";
				break;
			case LANGUAGE.Russian:
				result = "ru";
				break;
			case LANGUAGE.Spanish:
				result = "sp";
				break;
			case LANGUAGE.English:
				break;
			default:
				break;
			}

			return result;
		}
	}
}
