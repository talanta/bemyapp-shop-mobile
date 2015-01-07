using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;

namespace weshop.portable.ViewModels
{
	public class WishlistViewModel
		: MvxViewModel
	{
		public const string TYPENAME = "WishlistViewModel";

		IList<Product> _productCache;
		IDialogService _dialogService;
		IWishListService _wishlistService;
		IMvxMessenger _messenger;

		IMvxCommand selectCmd;

		public IMvxCommand SelectCmd {get {return selectCmd ?? (selectCmd = new MvxCommand<ProductViewModel>(OnSelect)); }}

		public IList<ProductViewModel> Products { get; private set; }

		public bool NoItemVisibile { get { return Products.Count == 0; } }

		public WishlistViewModel ()
		{
			Products = new ObservableCollection<ProductViewModel> {		
			};
		}

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _dialogService) {
				_dialogService = Mvx.Resolve<IDialogService> ();
				_wishlistService = Mvx.Resolve<IWishListService> ();
				_messenger = Mvx.Resolve<IMvxMessenger> ();

				_messenger.Subscribe<WishlistMessage> (OnWishlistMessage);
			}
			OnWishlistMessage (null);
		}

		public void OnSelect(ProductViewModel productViewModel)
		{
			ShowViewModel<DetailsViewModel> (productViewModel.Product);
		}

		protected void OnWishlistMessage(WishlistMessage msg)
		{
			_productCache = _wishlistService.GetAllProducts ();
			Products.Clear ();
			foreach (var p in _productCache) {
				Products.Add (new ProductViewModel { Product = p });
			}
			RaisePropertyChanged (() => NoItemVisibile);
		}
	}
}

