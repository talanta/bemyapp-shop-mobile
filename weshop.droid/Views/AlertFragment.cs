
using Android.App;
using Android.Content;
using Android.OS;
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

