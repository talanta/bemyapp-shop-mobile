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
}

