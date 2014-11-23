using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace weshop.portable
{
	public class BestOffer
	{
		[PrimaryKey]
		public string Id { get; set; }

		[Indexed]
		public string ProductId { get; set; }

		public string ProductURL { get; set; }

		public double SalePrice { get; set; }
	}
}

