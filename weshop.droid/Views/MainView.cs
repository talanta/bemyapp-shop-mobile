
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.Views;
using Android.OS;

namespace weshop.droid
{
	public class MainView : MvxFragment
	{
		BindableViewPager _viewPager;
		MvxBindablePagerAdapter _pagerAdapter;

		public MainView ()
		{
			this.RetainInstance = true;
		}

		protected void onViewPagerInstanciateItem (object sender, InstanciateItemEventArgs args)
		{
			_viewPager.setObjectForPosition (args.View, args.Position);
		}

		protected void onViewPagerDestroyItem (object sender, DestroyItemEventArgs args)
		{
			args.ViewGroup.RemoveView (_viewPager.findViewFromObject (args.Position));
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = this.BindingInflate (Resource.Layout.view_main, null);

			_viewPager = view.FindViewById<BindableViewPager> (Resource.Id.product_pager);
			_viewPager.setTransitionEffect (TransitionEffect.Stack);
			_pagerAdapter = new MvxBindablePagerAdapter (Activity, 
				(IMvxAndroidBindingContext)this.BindingContext);

			_pagerAdapter.InstanciateItemEvent += onViewPagerInstanciateItem;
			_pagerAdapter.DestroyItemEvent += onViewPagerDestroyItem;
			_viewPager.Adapter = _pagerAdapter;

			return view;
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.search, menu);
	
		}
	}
}

