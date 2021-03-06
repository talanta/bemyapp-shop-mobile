﻿using System;
using System.Threading.Tasks;

namespace weshop.portable
{
	public interface IDiscountService
	{
		string GetLastError ();

		ProductResult GetLastResult ();

		Task<bool> AppendResultFromSearchProdudct(SearchRequest request);

		Task<ProductResult> SearchProdudct(SearchRequest request);

		Task<ProductResult> GetProduct(ProductRequest request);
	}
}

