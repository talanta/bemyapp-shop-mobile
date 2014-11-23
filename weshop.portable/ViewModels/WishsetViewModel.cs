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
			ShowViewModel<FirstViewModel> ();
		}

	}
}

