using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using weshop.portable.ViewModels;
using Android.Content.PM;
using weshop.droid.Helpers;

namespace weshop.droid.Views
{
	[Activity(Label = "Wishop", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class FirstView : MvxActionBarActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			var view = this.LayoutInflater.Inflate(Resource.Layout.actioinbar, null);
			this.ActionBar.SetDisplayShowCustomEnabled (true);
			this.ActionBar.SetDisplayShowTitleEnabled(false);
			//this.ActionBar.SetDisplayShowHomeEnabled (false);

			this.ActionBar.SetIcon (Resource.Drawable.magnify);
			this.ActionBar.SetDisplayHomeAsUpEnabled (true);
			this.ActionBar.SetCustomView (Resource.Layout.actioinbar);
            SetContentView(Resource.Layout.FirstView);
        }

		public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.main, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Android.Resource.Id.Home:
				(this.ViewModel as FirstViewModel).GoToWishSet();
				break;
			case Resource.Id.menu_item_switch:
				(this.ViewModel as FirstViewModel).GoToWishList();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected(item);
		}
    }
}