using System;
using weshop.portable;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;

namespace weshop.droid
{
	public class DialogService 
		: IDialogService
	{
		#region IDialogService implementation

		public void ShowProduct (string url)
		{
	
			Intent i = new Intent(Intent.ActionView);
			i.SetData (Android.Net.Uri.Parse (url));
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;

			activity.StartActivity (i);

		//	startActivity(i);
		}

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


		public void ToastSuccess (string message, int xtimeout = 2000)
		{
			//TimeSpan.FromMilliseconds(
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;
				AndroidHUD.AndHUD.Shared.ShowSuccessWithStatus (activity, message, 
				AndroidHUD.MaskType.Black
					, TimeSpan.FromMilliseconds(xtimeout));
		}

		public void ToastError (string message, int xtimeout = 2000)
		{
			//TimeSpan.FromMilliseconds(
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;
			AndroidHUD.AndHUD.Shared.ShowErrorWithStatus (activity, message, 
				AndroidHUD.MaskType.Black
				, TimeSpan.FromMilliseconds(xtimeout));
		}

		#endregion


	}
}

