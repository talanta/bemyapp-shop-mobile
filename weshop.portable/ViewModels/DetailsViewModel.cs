using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;

namespace weshop.portable
{
	public class DetailsViewModel 
		: MvxViewModel
	{
		IDiscountService _discountService;
		IDialogService _dialogService;

		private IMvxCommand visitCmd;

		public IMvxCommand VisitCmd { get {return visitCmd ?? ( visitCmd = new MvxCommand(OnVisit)); } }
		public Product Product { get; set; }

		protected async override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _discountService) {
				_discountService = Mvx.Resolve<IDiscountService> ();
				_dialogService = Mvx.Resolve<IDialogService> ();
			}
			if (parameters == null || !parameters.Data.ContainsKey ("Id"))
				return;

			var productResult = await _discountService.GetProduct(new ProductRequest{ ProductIdList= new string[]{ parameters.Data["Id"] }});

			if (productResult == null || productResult.Products.Count == 0)
				return;
			this.Product = productResult.Products [0];
		}

		protected void OnVisit()
		{
			_dialogService.ShowProduct (Product.BestOffer.ProductURL);
		}

	}
}

