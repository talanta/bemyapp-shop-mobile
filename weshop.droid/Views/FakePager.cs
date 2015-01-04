using System;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace weshop.droid
{
	public class FakePager : PagerAdapter
	{
		JazzyViewPager _viewPager;
		Android.Support.V4.App.FragmentActivity _activity;
		int[] _img;

		public FakePager (Android.Support.V4.App.FragmentActivity activity, JazzyViewPager vp, int img1, int img2, int img3, int img4)
		{
			_activity = activity;
			_img = new int[]{ img1, img2, img3, img4 };
			_viewPager = vp;
		}
			

		public override void DestroyItem (ViewGroup container, int position, Java.Lang.Object @object)
		{
			container.RemoveView(_viewPager.findViewFromObject(position));
		}


		public override Java.Lang.Object InstantiateItem (ViewGroup viewgroup, int position)
		{
			//return base.InstantiateItem (container, position);
			var layout = _activity.LayoutInflater.Inflate (Resource.Layout.item_pager, null);
			//var tv = layout.FindViewById<TextView>(Resource.Id.tv_count);
			//var imageview = new ImageView(_activity);
			//imageview.SetBackgroundResource(_img[position % 4]);
			viewgroup.AddView(layout, -1, -1);
			//tv.Text = position.ToString();
			_viewPager.setObjectForPosition(layout, position);
			return layout;
		}

		public override bool IsViewFromObject (View view, Java.Lang.Object @object)
		{
			if (!(view is OutlineContainer))
				return view == @object; 
				
			return ((OutlineContainer)view).GetChildAt(0) == @object;
		}

		public override int Count {get {return 20;}}
			
	}
}

