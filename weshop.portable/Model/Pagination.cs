using System;

namespace weshop.portable
{
	public class Pagination
	{
		public int ItemsPerPage{ get; set; }
		public int PageNumber{ get; set; }

		public Pagination ()
		{
			ItemsPerPage = 10;
		}
	}
}

