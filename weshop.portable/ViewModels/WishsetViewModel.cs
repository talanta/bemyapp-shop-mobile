using System;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable.ViewModels
{
	public class WishsetViewModel
		: MvxViewModel
	{
		public const string TYPENAME = "WishsetViewModel";

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);
		}

		public void GoToMeet()
		{
			var bundle = new MvxBundle ();

			bundle.Data.Add ("keyword", "tenues sexy");
			ShowViewModel<FirstViewModel> (bundle);
		}

	}
}

