using System;

namespace weshop.portable
{
	public class SearchRequest
	{
		public string Keyword { get; set;}
		public Pagination Pagination {get;set;}

		public SearchRequest ()
		{
			Pagination = new Pagination ();
		}

	}

	public class ProductScope
	{
		public bool Offers { get; set; }
		public bool AssociatedProducts { get; set; }
		public bool Images { get; set; }
		public bool Ean { get; set; }
	}

	public class ProductRequest
	{
		public string [] ProductIdList {get;set;}

		public ProductScope Scope {get;set;}

		public ProductRequest ()
		{
			Scope = new ProductScope ();
		}
	}
}

