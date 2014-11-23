using System;
using System.Threading.Tasks;

namespace weshop.portable
{
	public interface IDiscountService
	{
	 Task<ProductResult> 
				SearchProdudct(SearchRequest request);
	}
}

