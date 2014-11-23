using System;
using Cirrious.MvvmCross.Droid.Views;
using Android.App;
using Android.OS;

namespace weshop.droid
{
	[Activity(Label = "Wishlist", Theme = "@style/AppTheme")]
	public class WishlistView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.view_wishlist);
		}
	}
}

