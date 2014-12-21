using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable
{
	public class MainViewModel 
		: MvxViewModel
	{
		public const string TYPENAME = "MainViewModel";

		IDiscountService _discountService;
		IDialogService _dialogService;
		IWishListService _wishlistService;

		public int CurrentIndex { get; set; }

		int itemPerPage = 20;

		string keyword = "Vetement";

		//private IList<Product> _products;
		public IList<Product> Products { get; private set; }

		public Product CurrentProduct { get; set; }


		private IMvxCommand likeCmd;
		private IMvxCommand dislikeCmd;
		private IMvxCommand pageSelectedCmd;
		private IMvxCommand showProductCmd;

		public IMvxCommand ShowProductCmd {get{ return showProductCmd ?? ( showProductCmd = new MvxCommand(OnShowProduct));  }}

		//public IMvxCommand DislikeCmd { get { return dislikeCmd ?? (dislikeCmd = new MvxCommand (OnDislike)); } }

		public IMvxCommand LikeCmd { get { return likeCmd ?? (likeCmd = new MvxCommand (OnLike)); } }

		public IMvxCommand PageSelectedCmd { get { return pageSelectedCmd ?? (pageSelectedCmd = new MvxCommand<int> (OnPageChanged)); } }

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null != _discountService)
				return;

			_discountService = Mvx.Resolve<IDiscountService> ();
			_dialogService = Mvx.Resolve<IDialogService> ();
			_wishlistService = Mvx.Resolve<IWishListService> ();

			if (Products != null && Products.Count > 0)
				return;
			if (parameters != null && parameters.Data != null && parameters.Data.Count > 0) {
				keyword = "tenues sexy";
			}
			await DisplayNextProduct ();
		}

		protected void OnPageChanged (int param)
		{
			CurrentIndex = param;
			if (CurrentIndex > 0)
				DisplayNextProduct ();
			else {
				CurrentProduct = this.Products [CurrentIndex];
				RaisePropertyChanged (() => CurrentProduct);
			}
		}

		protected async Task RetrieveItems ()
		{
			_dialogService.ShowProgress ();
			var request = new SearchRequest { 
				Keyword = keyword
			};
			request.Pagination.ItemsPerPage = itemPerPage;
			request.Pagination.PageNumber = CurrentIndex / itemPerPage;

			var products = await _discountService.SearchProdudct (request);

			_dialogService.Dismiss ();

			if (products == null || products.ItemCount == 0) {
				_dialogService.ToastError (_discountService.GetLastError (), 10000);
				return;
			}
			_wishlistService.FillLikes (products.Products);
			CurrentIndex = 0;
			this.Products = products.Products;
			RaisePropertyChanged (() => this.Products);
		}

		protected async Task DisplayNextProduct ()
		{
			if (CurrentIndex % itemPerPage == 0) {
				await RetrieveItems ();
				if (this.Products == null)
					return;
			}

			CurrentProduct = this.Products [CurrentIndex];

			RaisePropertyChanged (() => CurrentProduct);
		}


		protected async void OnLike ()
		{
			if (CurrentProduct.Like.HasValue && CurrentProduct.Like.Value) {	
				// action on like
				_wishlistService.AddItem (CurrentProduct);
				_dialogService.ToastSuccess ("Ce produit a été ajouté à votre wishlist");
				return;
			}
			if (CurrentProduct.Like.HasValue && !CurrentProduct.Like.Value) {
				_wishlistService.RemoveItem (CurrentProduct);
			}
			//await Task.Delay (2000);
			//DisplayNextProduct ();
		}

//		protected void OnDislike ()
//		{
//			DisplayNextProduct ();
//			// action on Dislike
//		}

		protected void OnShowProduct()
		{
			ShowViewModel<DetailsViewModel> (CurrentProduct);
		}
	}
}

