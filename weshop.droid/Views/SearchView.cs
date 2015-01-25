using Android.App;
using Android.Content.PM;
using Android.OS;
using weshop.portable;
using weshop.droid.Helpers;

namespace weshop.droid
{
	[Activity (Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]	
	[IntentFilter (new [] { "android.intent.action.SEARCH" })]
	public class SearchView : MvxActionBarActivity
	{
		protected override int ToolbarResourceId { get { return Resource.Id.toolbar_search; } }

		protected override int LayoutResource { get { return Resource.Layout.view_search; } }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.SupportActionBar.SetHomeButtonEnabled (true);
			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);	
		}

		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.search_activity, menu);
			var item = menu.FindItem (Resource.Id.menu_search);
			//var searchManager = (Android.App.SearchManager)GetSystemService (Android.Content.Context.SearchService);
			var searchView = (Android.Widget.SearchView)item.ActionView;
			//var searchInfo = searchManager.GetSearchableInfo (ComponentName);
			//searchView.SetSearchableInfo (searchInfo);
			searchView.SetIconifiedByDefault (false);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				(this.ViewModel as SearchViewModel).Close ();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}

