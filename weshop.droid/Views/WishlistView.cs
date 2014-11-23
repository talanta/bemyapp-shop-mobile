using System;
using Cirrious.MvvmCross.Droid.Views;
using Android.App;
using Android.OS;
using Android.Content.PM;
using weshop.portable;
using weshop.droid.Helpers;
using weshop.portable.ViewModels;

namespace weshop.droid
{
	[Activity (Label = "Meet", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class WishlistView : MvxActionBarActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetDisplayShowHomeEnabled (false);

			this.SetContentView (Resource.Layout.view_wishlist);

		}



		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				(this.ViewModel as WishlistViewModel).GoToMeet ();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected (item);

		}
	}
}