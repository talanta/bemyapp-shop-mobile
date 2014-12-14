using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Droid.Views;

namespace weshop.droid
{
	public class CustomPresenter : MvxAndroidViewPresenter, ICustomPresenter
	{
		// map between view-model and fragment host which creates and shows the view based on the view-model type
		private Dictionary<Type, IFragmentHost> dictionary = new Dictionary<Type, IFragmentHost>();

		public override void Show(MvxViewModelRequest request)
		{
			IFragmentHost host;
			if (this.dictionary.TryGetValue(request.ViewModelType, out host))
			{
				if (host.Show(request))
				{
					return;
				}
			}
			base.Show(request);
		}

		public void Register(Type viewModelType, IFragmentHost host)
		{
			this.dictionary[viewModelType] = host;
		}
	}
}

