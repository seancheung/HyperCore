using System;
using System.Data.Linq.Mapping;

namespace HyperCore.Common
{
	[Table(Name = "Set")]
	public class SetInfo
	{
		/// <summary>
		/// Set name
		/// </summary>
		[Column(Name = "SetName", IsPrimaryKey = true)]
		public string SetName
		{
			get;
			set;
		}

		/// <summary>
		/// Set code
		/// </summary>
		[Column(Name = "SetCode")]
		public string SetCode
		{
			get;
			set;
		}

		/// <summary>
		/// Last update time
		/// </summary>
		[Column(Name = "LastUpdate")]
		public DateTime LastUpdate
		{
			get;
			set;
		}

		/// <summary>
		/// Whether this set is stored locally
		/// </summary>
		[Column(Name = "Local")]
		public bool Local
		{
			get;
			set;
		}

		/// <summary>
		/// Set full name as 'Theros(THS)'
		/// </summary>
		public string FullName
		{
			get
			{
				return String.Format("{0}({1})", SetName, SetCode);
			}
		}
	}
}
