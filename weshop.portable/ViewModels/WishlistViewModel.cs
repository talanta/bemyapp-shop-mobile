using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;

namespace weshop.portable.ViewModels
{
	public class WishlistViewModel
		: MvxViewModel
	{
		readonly IDialogService _dialogService;
		readonly IWishListService _wishlistService;

		public IList<ProductViewModel> Products { get; private set; }

		public WishlistViewModel (IDialogService dialogService,
			IWishListService wishlistService)
		{
			_dialogService = dialogService;
			_wishlistService = wishlistService;

			Products = new List<ProductViewModel> ();
		}

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);


			var ps = _wishlistService.GetAllProducts ();
			foreach (var p in ps) {
				Products.Add (new ProductViewModel { Product = p });
			}
		}

		public void GoToMeet()
		{
			Close (this);
		}
	}
}

