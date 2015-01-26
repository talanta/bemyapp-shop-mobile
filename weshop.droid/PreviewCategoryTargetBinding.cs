using System;
using Android.Support.V7.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;
using weshop.portable;
using Android.Widget;

namespace weshop.droid
{
	public class PreviewCategoryTargetBinding 
		: MvxAndroidTargetBinding
	{
		public override Type TargetType { get { return typeof(Category); } }

		public override MvxBindingMode DefaultMode { get { return  MvxBindingMode.OneWay; } }

		public PreviewCategoryTargetBinding (Android.Widget.LinearLayout target)
			: base (target){ }
			
		public override void SetValue (object value)
		{
			base.SetValue (value);
		}

		protected override void SetValueImpl (object target, object value)
		{
			if (null == value)
				return;
			var view = (Android.Widget.LinearLayout)target;
//			var toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.preview_toolbar);
			var category = (Category)value;
			if (view.GetChildAt (0) is Android.Support.V7.Widget.Toolbar)
				return;

			var toolbar = new Android.Support.V7.Widget.Toolbar (view.Context);
			toolbar.LayoutParameters= (new LinearLayout.LayoutParams (
				LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent));
			view.AddView (toolbar, 0);


			toolbar.Title = category.Display;
			toolbar.Subtitle = "information a completer";
		}
	}
}

