using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace weshop.droid
{
	public class DetailsView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.view_details);
		}
	}
}

