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
		readonly List<Category> _categories = new List<Category> ();

		int itemPerPage = 20;

		string keyword = "Vetement";

		IDiscountService _discountService;
		IDialogService _dialogService;
		IWishListService _wishlistService;

		public IList<Product> Products { get; private set; }

		public List<Category> Categories { get { return _categories; } }

		public int CurrentIndex { get; set; }

		public Product CurrentProduct { get; set; }

		public Category SelectedCategory { get; set; }

		public string SearchInput { get; set; }

		public bool ShowLike { 
			get {
				if (CurrentProduct == null)
					return true;
				return !CurrentProduct.Like.HasValue || !CurrentProduct.Like.Value;
			} 
		}

		private IMvxCommand likeCmd;
		//private IMvxCommand dislikeCmd;
		private IMvxCommand pageSelectedCmd;
		private IMvxCommand showProductCmd;
		private IMvxCommand selectCategoryCmd;

		public IMvxCommand ShowProductCmd { get { return showProductCmd ?? (showProductCmd = new MvxCommand (OnShowProduct)); } }

		//public IMvxCommand DislikeCmd { get { return dislikeCmd ?? (dislikeCmd = new MvxCommand (OnDislike)); } }

		public IMvxCommand LikeCmd { get { return likeCmd ?? (likeCmd = new MvxCommand (OnLike)); } }

		public IMvxCommand PageSelectedCmd { get { return pageSelectedCmd ?? (pageSelectedCmd = new MvxCommand<int> (OnPageChanged)); } }

		public IMvxCommand SelectCategoryCmd { get { return selectCategoryCmd ?? (selectCategoryCmd = new MvxCommand<Category> (OnSelectCategory)); } }


		public MainViewModel ()
		{
			var cat = Newtonsoft.Json.JsonConvert.DeserializeObject<Category[]> (weshop.portable.Categories.JSON);
			this.Categories.AddRange (cat);
		}

		protected async void OnSelectCategory (Category cat)
		{
			if (cat == SelectedCategory)
				return;
			if (null != SelectedCategory)
				SelectedCategory.IsVisible = false;
			SelectedCategory = cat;
			cat.IsVisible = true;

			CurrentIndex = 0;

			RaisePropertyChanged (() => CurrentIndex);
			await DisplayNextProduct ();
			RaisePropertyChanged (() => SelectedCategory);
		}

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

			OnSelectCategory (Categories [0]);
		}

		protected void OnPageChanged (int param)
		{
			CurrentIndex = param;
			if (CurrentIndex > 0)
				DisplayNextProduct ();
			else {
				CurrentProduct = this.Products [CurrentIndex];
				RaisePropertyChanged (() => CurrentProduct);
				RaisePropertyChanged (() => ShowLike);
			}
		}

		protected async Task RetrieveItems ()
		{
			_dialogService.ShowProgress ();
			var request = new SearchRequest { Keyword = SelectedCategory.Keyword };
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

			RaisePropertyChanged (() => ShowLike);
			RaisePropertyChanged (() => CurrentProduct);
		}


		protected void OnLike ()
		{
			if (!CurrentProduct.Like.HasValue || !CurrentProduct.Like.Value) {	
				// action on like
				CurrentProduct.Like = true;
				_wishlistService.AddItem (CurrentProduct);
				_dialogService.ToastSuccess ("Ce produit a été ajouté à votre wishlist");

			} else {
				_wishlistService.RemoveItem (CurrentProduct);
				CurrentProduct.Like = null;
			}
			RaisePropertyChanged (() => ShowLike);
			//await Task.Delay (2000);
			//DisplayNextProduct ();
		}


		protected void OnShowProduct ()
		{
			ShowViewModel<DetailsViewModel> (CurrentProduct);
		}

		public async void Search ()
		{
			_dialogService.ShowProgress ();
			var request = new SearchRequest { Keyword = SearchInput };
			request.Pagination.ItemsPerPage = itemPerPage;
			request.Pagination.PageNumber = CurrentIndex / itemPerPage;

			bool hasresult = await _discountService.AppendResultFromSearchProdudct (request);

			_dialogService.Dismiss ();

			if (!hasresult)
				return;
			ShowViewModel<SearchViewModel> ();
		}
	}
}

