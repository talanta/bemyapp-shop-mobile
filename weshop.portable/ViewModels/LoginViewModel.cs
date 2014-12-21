using System;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.portable
{
	public class LoginViewModel 
		: MvxViewModel
	{

		private IMvxCommand closeCmd;
		public IMvxCommand CloseCmd { get {return closeCmd ?? ( closeCmd = new MvxCommand(OnClose)); } }


		protected void OnClose()
		{
			this.Close (this);
		}
	}
}

