
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

using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace weshop.droid
{
	public class AlertFragment : MvxDialogFragment
	{
		readonly int _layoutId;

		public AlertFragment(int layoutId)
		{
			_layoutId = layoutId;
		}

//		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
//		{
//
//			return view;
//		}

		public override Dialog OnCreateDialog(Bundle savedState)
		{
			base.EnsureBindingContextSet(savedState);

			AlertDialog.Builder builder = new AlertDialog.Builder (Activity);
			var view = this.BindingInflate(_layoutId, null);
			builder.SetView (view);
			builder.SetNegativeButton ("OK",(IDialogInterfaceOnClickListener) null);
			return builder.Create ();
		}
	}
}

