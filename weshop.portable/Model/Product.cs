using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Collections.Generic;

namespace weshop.portable
{
	public class Product
	{
		public Product ()
		{
			Sizes = new List<Size> ();
		}

		[Ignore]
		public BestOffer BestOffer { get; set; }
		[Ignore]
		public IList<Size> Sizes { get; private set; }

		public string MainImageUrl { get; set; }

		public string Name { get; set; }

		public string Description { get; set;}

		[PrimaryKey]
		public string Id { get; set; }

		public bool? Like { get; set; }
	}
}

