using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;

namespace weshop.portable
{
	public class ProductViewModel
	{
		public Product Product { get; set;}

		IMvxCommand showCmd;
		public IMvxCommand ShowCmd{ get {return showCmd ??(showCmd = new MvxCommand(OnShowCmd)); } }

		protected void OnShowCmd()
		{
			Mvx.Resolve<IDialogService> ()
				.ShowProduct (Product.BestOffer.ProductURL);
		}
	}
}


