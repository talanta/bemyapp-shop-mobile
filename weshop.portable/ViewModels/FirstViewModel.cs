using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace weshop.portable.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
		readonly IDiscountService _discountService;
		readonly IDialogService _dialogService;
		readonly IWishListService _wishlistService;

		int index = 0;
		int itemPerPage = 10;

		private IList<Product> _products;

		public Product CurrentProduct { get; set; }

		private string _hello = "Hello MvvmCross";
        public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }
		}

		private IMvxCommand likeCmd;
		private IMvxCommand dislikeCmd;

		public IMvxCommand DislikeCmd { get {return dislikeCmd ?? (dislikeCmd = new MvxCommand (OnDislike)); } }
		public IMvxCommand LikeCmd { get { return likeCmd ?? (likeCmd = new MvxCommand(OnLike)); } }

		public FirstViewModel (IDiscountService discountService,
			IDialogService dialogService,
			IWishListService wishlistService)
		{
			_discountService = discountService;
			_dialogService = dialogService;
			_wishlistService = wishlistService;
		}

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);
		   

			await DisplayNextProduct ();
		}

		protected async Task RetrieveItems()
		{
			_dialogService.ShowProgress ();
			var request = new SearchRequest { 
				Keyword = "Vetements"
			};
			request.Pagination.ItemsPerPage = itemPerPage;
			request.Pagination.PageNumber = index / itemPerPage;
			var products = await _discountService.SearchProdudct(request);


			_dialogService.Dismiss ();

			if (products == null || products.ItemCount == 0) {
				Mvx.Trace ("Returned nothing");
				return;
			}
			index = 0;
			_products = products.Products;
		}

		protected async Task DisplayNextProduct()
		{
			if (index % itemPerPage == 0) {
				await RetrieveItems ();
			}
			CurrentProduct = _products [index++];

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

		public void GoToWishList()
		{
			ShowViewModel<WishlistViewModel> ();
		}
    }
}
