using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using weshop.portable;
using weshop.portable.ViewModels;
using weshop.droid.Helpers;

namespace weshop.droid.Views
{
	[Activity (Icon = "@drawable/logob", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class FirstView : MvxActionBarActivity, IFragmentHost
	{
		private string _title;
		private string _drawerTitle;
		private DrawerLayout _drawer;
		private MvxListView _drawerList;
		private View _leftDrawer;
		private MyActionBarDrawerToggle _drawerToggle;
		private FirstViewModel m_ViewModel;

		public new FirstViewModel ViewModel {
			get { return this.m_ViewModel ?? (this.m_ViewModel = base.ViewModel as FirstViewModel); }
		}

		protected override int LayoutResource{ get { return  Resource.Layout.FirstView; } }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);

			this.SetSupportActionBar (toolbar);

			this._drawer = this.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			this._drawerList = this.FindViewById<MvxListView> (Resource.Id.drawer_list);
			this._leftDrawer = this.FindViewById<View> (Resource.Id.left_drawer);

			this._drawer.SetDrawerShadow (Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);
			//DrawerToggle is the animation that happens with the indicator next to the
			//ActionBar icon. You can choose not to use this.
			this._drawerToggle = new MyActionBarDrawerToggle (this, this._drawer,
				toolbar,
				//Resource.Drawable.ic_drawer_light,
				Resource.String.drawer_open,
				Resource.String.drawer_close);
			//You can alternatively use _drawer.DrawerClosed here
			this._drawerToggle.DrawerClosed += delegate {
				this.SupportActionBar.Title = this._title;
				this.InvalidateOptionsMenu ();
			};

			//You can alternatively use _drawer.DrawerOpened here
			this._drawerToggle.DrawerOpened += delegate {
				this.SupportActionBar.Title = this._drawerTitle;
				this.InvalidateOptionsMenu ();
			};

			this._drawer.SetDrawerListener (this._drawerToggle);

			this.SupportActionBar.SetHomeButtonEnabled (true);

			this.RegisterForDetailsRequests ();

			if (null == bundle) {
				this.ViewModel.SelectMenuItemCommand.Execute (this.ViewModel.MenuItems [0]);
			}
		}

		/// <summary>
		/// Use the custom presenter to determine if we can navigate forward.
		/// </summary>
		private void RegisterForDetailsRequests ()
		{
			var customPresenter = Mvx.Resolve<ICustomPresenter> ();
			customPresenter.Register (typeof(MainViewModel), this);
			customPresenter.Register (typeof(WishsetViewModel), this);
			customPresenter.Register (typeof(WishlistViewModel), this);
		}

		/// <summary>
		/// Read all about this, but this is a nice way if there were multiple
		/// fragments on the screen for us to decide what and where to show stuff
		/// See: http://enginecore.blogspot.ro/2013/06/more-dynamic-android-fragments-with.html
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public bool Show (MvxViewModelRequest request)
		{
			try {
				var frag = this.SupportFragmentManager.FindFragmentById (Resource.Id.content_frame) as MvxFragment;
				if (frag != null && frag.ViewModel.GetType () == request.ViewModelType) {
					return true;
				}
				var fragmentTransaction = this.SupportFragmentManager.BeginTransaction ();
//
				if (request.ViewModelType == typeof(MainViewModel)) {
					_title = GetString (Resource.String.nav_home);
					frag = new MainView {ViewModel = ViewModel.MainViewModel};
					fragmentTransaction = fragmentTransaction.Replace (Resource.Id.content_frame, frag);
					ViewModel.MainViewModel.Init (null);
				} else if (request.ViewModelType == typeof(WishsetViewModel)) {
					_title = GetString (Resource.String.nav_categories);
					frag = new WishsetView {ViewModel = ViewModel.WishSetViewModel};
					this._drawerList.SetItemChecked (this.ViewModel.MenuItems.FindIndex (m => m.Section == request.ViewModelType), true);
					fragmentTransaction = fragmentTransaction.Replace (Resource.Id.content_frame, frag);
					ViewModel.WishSetViewModel.Init (null);
				} else if (request.ViewModelType == typeof(WishlistViewModel)) {
					_title = GetString (Resource.String.nav_list);
					frag = new WishlistView {ViewModel = ViewModel.WishlistViewModel};

					this._drawerList.SetItemChecked (this.ViewModel.MenuItems.FindIndex (m => m.Section == request.ViewModelType), true);
					fragmentTransaction = fragmentTransaction.Replace (Resource.Id.content_frame, frag);
					ViewModel.WishlistViewModel.Init (null);
				}
//				fragmentTransaction = fragmentTransaction
//					.SetCustomAnimations(Resource.Animation.slide_in_bottom,Android.Resource.Animation.FadeOut,Android.Resource.Animation.FadeIn, Resource.Animation.slide_out_bottom)
//					.Replace(Resource.Id.content_frame, frag)
//					.AddToBackStack(title)
				//Normally we would do this, but we already have it
				fragmentTransaction.Commit ();
				return true;
			} catch (RemoteException ex) {
				string str = ex.Message;
				return false;
			} finally {
				this._drawer.CloseDrawer (this._leftDrawer);
			}
		}


		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.main, menu);
		
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			this._drawerToggle.SyncState ();
		}

		public override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			this._drawerToggle.OnConfigurationChanged (newConfig);
		}

		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.menu_about:
				Mvx.Resolve<IDialogService> ().ShowAbout ();
				break;
			case Resource.Id.menu_search:
				this.ViewModel.GoToSearch ();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}