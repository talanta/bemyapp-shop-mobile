using System;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cirrious.CrossCore;

namespace weshop.portable
{
	public class MainViewModel 
		: MvxViewModel
	{
		public const string TYPENAME = "MainViewModel";

		IDiscountService _discountService;
		IDialogService _dialogService;
		IWishListService _wishlistService;

		int index = 0;
		int itemPerPage = 10;

		string keyword = "Vetement";

		//private IList<Product> _products;
		public IList<Product> Products { get; private set; }

		public Product CurrentProduct { get; set; }


		private IMvxCommand likeCmd;
		private IMvxCommand dislikeCmd;

		public IMvxCommand DislikeCmd { get {return dislikeCmd ?? (dislikeCmd = new MvxCommand (OnDislike)); } }
		public IMvxCommand LikeCmd { get { return likeCmd ?? (likeCmd = new MvxCommand(OnLike)); } }


	

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _discountService) {

				_discountService = Mvx.Resolve<IDiscountService>();
				_dialogService = Mvx.Resolve<IDialogService>();
				_wishlistService = Mvx.Resolve<IWishListService>();
			}


			if (parameters != null && parameters.Data != null && parameters.Data.Count > 0) {
				keyword = "tenues sexy";
			}
			//await DisplayNextProduct ();
		}
		protected async Task RetrieveItems()
		{
			_dialogService.ShowProgress ();
			var request = new SearchRequest { 
				Keyword = keyword
			};
			request.Pagination.ItemsPerPage = itemPerPage;
			request.Pagination.PageNumber = index / itemPerPage;
			var products = await _discountService.SearchProdudct(request);


			_dialogService.Dismiss ();

			if (products == null || products.ItemCount == 0) {
				_dialogService.ToastError (_discountService.GetLastError (), 10000);
				return;
			}
			index = 0;
			this.Products = products.Products;
			RaisePropertyChanged (() => this.Products);
		}

		protected async Task DisplayNextProduct()
		{
			if (index % itemPerPage == 0) {
				await RetrieveItems ();
				if (this.Products == null)
					return;
			}

			CurrentProduct = this.Products [index++];

			RaisePropertyChanged (() => CurrentProduct);
		}


		protected async void OnLike()
		{
			// action on like
			_wishlistService.AddItem (CurrentProduct);
			_dialogService.ToastSuccess("Ce produit a été ajouté à votre wishlist");

			await Task.Delay (2000);
			DisplayNextProduct ();

		}

		protected void OnDislike()
		{
			DisplayNextProduct ();
			// action on Dislike
		}


	}
}

