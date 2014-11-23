using System;
using Cirrious.MvvmCross.Droid.Views;
using Android.App;
using Android.OS;
using Android.Content.PM;

namespace weshop.droid
{
	[Activity(Label = "Wishlist", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class WishlistView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.view_wishlist);
		}
	}
}

