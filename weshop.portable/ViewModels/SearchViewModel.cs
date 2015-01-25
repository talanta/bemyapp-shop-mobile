using System;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable
{
	public class SearchViewModel
		:BaseViewModel
	{
		IDiscountService _discountService;

		IMvxCommand selectCmd;

		public IMvxCommand SelectCmd {get {return selectCmd ?? (selectCmd = new MvxCommand<ProductViewModel>(OnSelect)); }}

		public IList<ProductViewModel> Products { get; private set; }
			
		protected override void InitFromBundle (Cirrious.MvvmCross.ViewModels.IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);

			if (null == _discountService) {		
				_discountService = Mvx.Resolve<IDiscountService> ();
			}
			var lastResult = _discountService.GetLastResult ();
			if (lastResult == null)
				return;

			this.Products = lastResult.Products
				.Select (p => new ProductViewModel{ Product = p })
				.ToList();
		}

		protected void OnSelect(ProductViewModel productViewModel)
		{
			ShowViewModel<DetailsViewModel> (productViewModel.Product);
		}
	}
}

