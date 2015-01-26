using System;
using weshop.portable;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;
using Android.App;

namespace weshop.droid
{
	public class DialogService 
		: IDialogService
	{
		#region IDialogService implementation

		public void ShowAbout()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity 
				as weshop.droid.Helpers.MvxActionBarActivity;

			var df = new AlertFragment (Resource.Layout.dialog_about);
			df.Show (activity.SupportFragmentManager, "about");
		}

		public void ShowProduct (string url)
		{	
			Intent i = new Intent(Intent.ActionView);
			i.SetData (Android.Net.Uri.Parse (url));
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;

			activity.StartActivity (i);
		}

		public void ShowProgress ()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity;
			AndroidHUD.AndHUD.Shared
				.Show (activity);
		}

		public void Dismiss ()
		{
			AndroidHUD.AndHUD.Shared.Dismiss ();
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

