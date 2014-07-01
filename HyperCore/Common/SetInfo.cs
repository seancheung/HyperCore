using System;
using System.Data.Linq.Mapping;

namespace HyperCore.Common
{
	[Table(Name = "Set")]
	public class SetInfo
	{
		[Column(Name = "SetName", IsPrimaryKey = true)]
		public string SetName { get; set; }
		[Column(Name = "SetCode")]
		public string SetCode { get; set; }
		[Column(Name = "LastUpdate")]
		public DateTime LastUpdate { get; set; }
		[Column(Name = "Local")]
		public bool Local { get; set; }
	}
}
