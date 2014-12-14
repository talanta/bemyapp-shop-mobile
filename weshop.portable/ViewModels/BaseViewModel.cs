using System;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable
{
	public class BaseViewModel: MvxViewModel
	{
		private string _Title = string.Empty;
		/// <summary>
		/// Gets or sets the name of the menu
		/// </summary>
		public string Title
		{
			get { return this._Title; }
			set { this._Title = value; this.RaisePropertyChanged(() => this.Title); }
		}
	}
}

