using System;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable.ViewModels
{
	public class WishsetViewModel
		: MvxViewModel
	{
		public WishsetViewModel ()
		{

		}

		public void GoToMeet()
		{
			var bundle = new MvxBundle ();

			bundle.Data.Add ("keyword", "tenues sexy");
			ShowViewModel<FirstViewModel> (bundle);
		}

	}
}

