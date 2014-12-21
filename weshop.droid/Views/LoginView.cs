using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Android.App;
using Android.Content.PM;

namespace weshop.droid
{
	[Activity(Icon="@drawable/logob", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class LoginView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.view_login);
		}
	}
}

