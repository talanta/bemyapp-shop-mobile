using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace weshop.portable
{
	public class Product
	{
		[Ignore]
		public BestOffer BestOffer { get; set; }

		public string MainImageUrl { get; set; }

		public string Name { get; set; }

		[PrimaryKey]
		public string Id { get; set; }

		public bool? Like { get; set; }
	}
}

