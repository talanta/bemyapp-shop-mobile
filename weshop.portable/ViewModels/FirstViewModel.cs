using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;

namespace weshop.portable.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
		readonly IDiscountService _discountService;
		readonly IDialogService _dialogService;
		int index;

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

		public FirstViewModel (IDiscountService discountService,
			IDialogService dialogService)
		{
			_discountService = discountService;
			_dialogService = dialogService;
		}

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			_dialogService.ShowProgress ();
			var products = await _discountService.SearchProdudct(new SearchRequest
				{ 
					 Keyword = "Vetements"
				});

			_dialogService.Dismiss ();

			if (products == null || products.ItemCount == 0) {
				Mvx.Trace ("Returned nothing");
				return;
			}
			index = 0;
			_products = products.Products;
			DisplayNextProduct ();
		}

		protected void DisplayNextProduct()
		{
			CurrentProduct = _products [index++];

			RaisePropertyChanged (() => CurrentProduct);
		}


		protected void OnLike()
		{
			// action on like
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
