using System;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.droid
{
	public interface ICustomPresenter
	{
		void Register(Type viewModelType, IFragmentHost host);
	}

	public interface IFragmentHost
	{
		bool Show(MvxViewModelRequest request);
	}
}

