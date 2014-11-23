using System;
using System.Collections.Generic;

namespace weshop.portable
{
	public class ProductResult
	{
		public ProductResult ()
		{
			Products = new List<Product> ();
		}

		public int ItemCount { get; set; }

		public int PageCount { get; set; }

		public int PageNumber{ get; set; }

		public IList<Product> Products {get; private set;}
	}
}

