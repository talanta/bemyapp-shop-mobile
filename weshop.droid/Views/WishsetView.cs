
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Android.Content.PM;
using weshop.portable;
using weshop.portable.ViewModels;

namespace weshop.droid
{
	[Activity(Label = "", Icon = "@drawable/logob", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class WishsetView :MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			this.SetContentView (Resource.Layout.view_wishset);

		}	

		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.search, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{		
			case Resource.Id.menu_meet:
				(this.ViewModel as WishsetViewModel).GoToMeet();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected(item);
		}
	}
}

