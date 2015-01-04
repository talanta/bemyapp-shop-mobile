using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;

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

		public WishlistViewModel ()
		{
			Products = new System.Collections.ObjectModel.ObservableCollection<ProductViewModel> {		
			};
		}

		public bool NoItemVisibile { get { return Products.Count == 0; } }

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
		//	RaisePropertyChanged (() => NoItemVisibile);
		}

		public void GoToMeet()
		{
			Close (this);
		}

		public void OnSelect(ProductViewModel productViewModel)
		{
			ShowViewModel<DetailsViewModel> (productViewModel.Product);
		}

		protected void OnWishlistMessage(WishlistMessage msg)
		{
			_productCache = _wishlistService.GetAllProducts ();
			Products.Clear ();
			//Products = new System.Collections.ObjectModel.ObservableCollection<ProductViewModel> ();
			foreach (var p in _productCache) {
				Products.Add (new ProductViewModel { Product = p });
			}
			//RaisePropertyChanged (() => Products);
			RaisePropertyChanged (() => NoItemVisibile);
		}
	}
}

