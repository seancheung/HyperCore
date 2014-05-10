
namespace HyperCore.Data
{
	public class Card : HyperCore.Common.Card
	{
		/// <summary>
		/// the color of its B-side card(if double-faced)
		/// (use use ' ' as separator for multi-color, e.g. 'Red Blue')
		/// </summary>
		public string ColorBside { get; set; }

		public bool isdoubleface;

		public bool issplit;
	}
}
