using System;
using weshop.portable;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;

namespace weshop.droid
{
	public class DialogService 
		: IDialogService
	{
		#region IDialogService implementation

		public void ShowProgress ()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;
			AndroidHUD.AndHUD.Shared
				.Show (activity);
			//throw new NotImplementedException ();
		}

		public void Dismiss ()
		{
			AndroidHUD.AndHUD.Shared.Dismiss ();
			//throw new NotImplementedException ();
		}

		#endregion


	}
}

