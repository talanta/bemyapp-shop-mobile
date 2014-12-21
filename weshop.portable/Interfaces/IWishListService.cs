using System;
using System.Collections.Generic;

namespace weshop.portable
{
	public interface IWishListService
	{
		void FillLikes (IList<Product> products);

		void AddItem(Product item);

		void RemoveItem (Product item);

		IList<Product> GetAllProducts();
	}
}

