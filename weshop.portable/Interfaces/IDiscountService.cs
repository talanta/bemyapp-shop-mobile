using System;
using System.Threading.Tasks;

namespace weshop.portable
{
	public interface IDiscountService
	{
		string GetLastError ();

	 Task<ProductResult> 
				SearchProdudct(SearchRequest request);

		Task<ProductResult> GetProduct(ProductRequest request);
	}
}

