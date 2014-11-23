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

		public IList<Product> Products { get; private set; }

		public WishlistViewModel (IDialogService dialogService,
			IWishListService wishlistService)
		{
			_dialogService = dialogService;
			_wishlistService = wishlistService;
		}

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			Products = _wishlistService.GetAllProducts ();
		}

		public void GoToMeet()
		{
			Close (this);
		}
	}
}

