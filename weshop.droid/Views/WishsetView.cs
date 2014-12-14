
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using weshop.portable;
using weshop.portable.ViewModels;

namespace weshop.droid
{
	public class WishsetView :MvxFragment
	{
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			return this.BindingInflate (Resource.Layout.view_wishset, null);
		}
//		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
//		{
//			MenuInflater.Inflate (Resource.Menu.search, menu);
//			return base.OnCreateOptionsMenu (menu);
//		}
//
//		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
//		{
//			switch (item.ItemId) {		
//			case Resource.Id.menu_meet:
//				(this.ViewModel as WishsetViewModel).GoToMeet ();
//				break;
//			default:
//				break;
//			}
//			return base.OnOptionsItemSelected (item);
//		}
	}
}

