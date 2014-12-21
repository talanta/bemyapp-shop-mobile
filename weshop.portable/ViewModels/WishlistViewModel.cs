using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable.ViewModels
{
	public class WishlistViewModel
		: MvxViewModel
	{
		public const string TYPENAME = "WishlistViewModel";

		IDialogService _dialogService;
		IWishListService _wishlistService;

		IMvxCommand selectCmd;

		public IMvxCommand SelectCmd {get {return selectCmd ?? (selectCmd = new MvxCommand<ProductViewModel>(OnSelect)); }}

		public IList<ProductViewModel> Products { get; private set; }

		public WishlistViewModel ()
		{
			Products = new List<ProductViewModel> {		
			};
		}

		public bool NoItemVisibile { get { return Products.Count == 0; } }

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _dialogService) {
				_dialogService = Mvx.Resolve<IDialogService> ();
				_wishlistService = Mvx.Resolve<IWishListService> ();
			}

			var ps = _wishlistService.GetAllProducts ();
			Products.Clear ();
			foreach (var p in ps) {
				Products.Add (new ProductViewModel { Product = p });
			}
		}

		public void GoToMeet()
		{
			Close (this);
		}

		public void OnSelect(ProductViewModel productViewModel)
		{
			ShowViewModel<DetailsViewModel> (productViewModel.Product);
		}
	}
}

