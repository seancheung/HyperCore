using System.Data.Linq.Mapping;

namespace HyperCore.Common
{
	[Table(Name = "File")]
	public class FileData
	{
		[Column(Name = "id", IsPrimaryKey = true)]
		public string ID
		{
			get;
			set;
		}

		[Column(Name = "data")]
		public byte[] Data
		{
			get;
			set;
		}

		[Column(Name = "length")]
		public int Length
		{
			get;
			set;
		}
	}
}
