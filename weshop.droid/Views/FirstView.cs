using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using weshop.portable.ViewModels;
using Android.Content.PM;

namespace weshop.droid.Views
{
	[Activity(Label = "Wishop", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
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