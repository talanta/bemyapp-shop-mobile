using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace weshop.portable
{
	public class DetailsViewModel 
		: MvxViewModel
	{
		IDiscountService _discountService;
		IDialogService _dialogService;
		IWishListService _wishlistService;
		IMvxMessenger _messenger;

		private IMvxCommand visitCmd;
		private IMvxCommand dislikeCmd;
		private IMvxCommand cancelCmd;

		public IMvxCommand VisitCmd { get { return visitCmd ?? (visitCmd = new MvxCommand (OnVisit)); } }

		public IMvxCommand CancelCmd { get { return cancelCmd ?? (cancelCmd = new MvxCommand (OnCancel)); } }

		public IMvxCommand DislikeCmd { get { return dislikeCmd ?? (dislikeCmd = new MvxCommand (OnDislike)); } }


		public Product Product { get; set; }

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _discountService) {
				_discountService = Mvx.Resolve<IDiscountService> ();
				_dialogService = Mvx.Resolve<IDialogService> ();
				_wishlistService = Mvx.Resolve<IWishListService> ();
				_messenger = Mvx.Resolve<IMvxMessenger> ();
			}
			if (parameters == null || !parameters.Data.ContainsKey ("Id"))
				return;

			var productResult = await _discountService.GetProduct (new ProductRequest{ ProductIdList = new string[]{ parameters.Data ["Id"] } });

			if (productResult == null || productResult.Products.Count == 0)
				return;
			_wishlistService.FillLikes (productResult.Products);
			this.Product = productResult.Products [0];
			RaisePropertyChanged (() => this.Product);
		}

		protected void OnVisit ()
		{
			_dialogService.ShowProduct (Product.BestOffer.ProductURL);
		}

		protected void OnCancel ()
		{
			Close (this);
			_messenger.Publish (new WishlistMessage (this));
		}

		protected void OnDislike ()
		{
			_wishlistService.RemoveItem (Product);
			Product.Like = null;

			RaisePropertyChanged (() => this.Product);
			_dialogService.ToastSuccess ("Ce produit a été retiré de votre wishlist");
			// action on Dislike
		}

	}
}

