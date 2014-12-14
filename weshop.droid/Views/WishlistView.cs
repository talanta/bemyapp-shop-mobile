﻿using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using weshop.portable;
using weshop.portable.ViewModels;
using weshop.droid.Helpers;

namespace weshop.droid
{
	public class WishlistView : MvxFragment
	{
		public WishlistView ()
		{
			this.RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			return this.BindingInflate (Resource.Layout.view_wishlist, null);
		}
	}
}