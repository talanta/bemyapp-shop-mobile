using Android.App;
using Android.Content.PM;
using Android.OS;
using weshop.portable;
using weshop.droid.Helpers;

namespace weshop.droid
{
	[Activity (Label = "Paramètres", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SettingsView : MvxActionBarActivity
	{
		protected override int ToolbarResourceId { get { return Resource.Id.toolbar; } }

		protected override int LayoutResource{ get { return  Resource.Layout.view_settings; } }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			this.SupportActionBar.SetHomeButtonEnabled (true);
			this.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
		}

		public override bool OnOptionsItemSelected (Android.Views.IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				(this.ViewModel as SettingsViewModel).Close ();
				break;
			default:
				break;
			}
			return base.OnOptionsItemSelected (item);
		}
	}
}

