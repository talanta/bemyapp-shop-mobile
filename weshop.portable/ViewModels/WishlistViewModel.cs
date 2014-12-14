using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Cirrious.CrossCore;

namespace weshop.portable.ViewModels
{
	public class WishlistViewModel
		: MvxViewModel
	{
		public const string TYPENAME = "WishlistViewModel";

		IDialogService _dialogService;
		IWishListService _wishlistService;

		public IList<ProductViewModel> Products { get; private set; }

		public WishlistViewModel ()
		{
			Products = new List<ProductViewModel> ();
		}

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _dialogService) {
				_dialogService = Mvx.Resolve<IDialogService> ();
				_wishlistService = Mvx.Resolve<IWishListService> ();
			}

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

