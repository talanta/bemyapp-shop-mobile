
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Views;
using Android.OS;

namespace weshop.droid
{
	public class MainView : MvxFragment
	{
		public MainView ()
		{
			this.RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = this.BindingInflate (Resource.Layout.view_main, null);

			var viewPager = view.FindViewById<BindableViewPager> (Resource.Id.product_pager);
			viewPager.SetPageTransformer (true, new ZoomOutPageTransformer ());

			return view;
		}
	}
}

