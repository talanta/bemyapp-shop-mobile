using Android.Animation;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Java.Lang;
using weshop.droid.Helpers;
using Xam.UI;
using weshop.portable;

namespace weshop.droid
{
	internal interface IInterceptionTarget
	{
		float MinInterceptionLayoutY { get; }

		IScrollable Scrollable { get; }

		TouchInterceptionFrameLayout InterceptionLayout { get; }

		float ScrollYOnDownMotion { get; set; }

		void UpdateViews (float translationY, bool animated);

		int FlexibleSpaceImageHeight { get; }

		int HeaderBarHeight{ get; }
	}

	internal interface IChangeBackgroundTarget
	{
		void ChangeHeaderBackgroundHeight (float height, float to, float heightOnGapHidden);
	}

	[Activity(Icon="@drawable/logob", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class DetailsView : MvxActionBarActivity, IObservableScrollViewCallbacks, IChangeBackgroundTarget,IInterceptionTarget
	{
		#region implemented abstract members of MvxActionBarActivity

		protected override int ToolbarResourceId{ get { return 0;} }

		protected override int LayoutResource { get { return Resource.Layout.view_details;} }

		#endregion

		public IScrollable Scrollable { get; private set; }
		public TouchInterceptionFrameLayout InterceptionLayout { get; private set; }
		public float ScrollYOnDownMotion { get; set;}
		public int FlexibleSpaceImageHeight { get; private set; }
		public int HeaderBarHeight { get; private set; }	

		protected View mHeader;
		protected View mHeaderBar;
		private View mImageView;
		private View mHeaderBackground;
		private TextView mTitle;

		// Fields that needs to saved
		private float mInitialTranslationY;

		// Fields that just keep constants like resource values
		protected int mActionBarSize;
		protected int mIntersectionHeight;

		// Temporary states

		private float mPrevTranslationY;
		private bool mGapIsChanging;
		private bool mGapHidden;
		private bool mReady;

		private ITouchInterceptionListener mInterceptionListener; 

		protected ObservableScrollView createScrollable ()
		{
			var scrollView =  FindViewById<ObservableScrollView>(Resource.Id.scroll);
			scrollView.SetScrollViewCallbacks(this);
			// recyclerView.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(this));
			// recyclerView.HasFixedSize=(true);
			// setDummyData(recyclerView);
			return scrollView;
		}	

		public void UpdateViews (float translationY, bool animated)
		{
			// If it's ListView, onScrollChanged is called before ListView is laid out (onGlobalLayout).
			// This causes weird animation when onRestoreInstanceState occurred,
			// so we check if it's laid out already.
			if (!mReady) {
				return;
			}
			InterceptionLayout.TranslationY = translationY;
			// Translate image
			mImageView.TranslationY = (translationY - (FlexibleSpaceImageHeight - HeaderBarHeight)) / 2;
			// Translate title
			mTitle.TranslationY = Java.Lang.Math.Min (mIntersectionHeight, (HeaderBarHeight - mActionBarSize) / 2);


			// Show/hide gap
			bool scrollUp = translationY < mPrevTranslationY;
			if (scrollUp) {
				if (translationY <= mActionBarSize) {
					changeHeaderBackgroundHeightAnimated(false, animated);
				}
			} else {
				if (mActionBarSize <= translationY) {
					changeHeaderBackgroundHeightAnimated(true, animated);
				}
			}
			mPrevTranslationY = translationY;
		}
		public float MinInterceptionLayoutY {
			get {
				return mActionBarSize - mIntersectionHeight;
				// If you want to move header bar to the top, return 0 instead.
				//return 0;
			}
		}

		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet ();
			var set = this.CreateBindingSet<DetailsView, DetailsViewModel> ();

			set.Bind (this).For (v => v.ProductName).To (m => m.Product.Name).Apply();
		}

		public string ProductName
		{
			get{ return Title;}
			set{ 
				if (mTitle == null)
					return;
				Title = value;
				mTitle.Text = value;
			}
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			if (null == mInterceptionListener)
				mInterceptionListener = new FillGapListener (this);
			// Create your application here
			FlexibleSpaceImageHeight = Resources.GetDimensionPixelSize(Resource.Dimension.flexible_space_image_height);
			mActionBarSize = GetActionBarSize();
			HeaderBarHeight = Resources.GetDimensionPixelSize(Resource.Dimension.header_bar_height);

			// Even when the top gap has began to change, header bar still can move
			// within mIntersectionHeight.
			mIntersectionHeight = Resources.GetDimensionPixelSize(Resource.Dimension.intersection_height);

			mImageView = FindViewById(Resource.Id.image);
			mHeader = FindViewById(Resource.Id.header);
			mHeaderBar = FindViewById(Resource.Id.header_bar);
			mHeaderBackground = FindViewById(Resource.Id.header_background);

			Scrollable = createScrollable();


			InterceptionLayout =  FindViewById<TouchInterceptionFrameLayout>(Resource.Id.scroll_wrapper);
			InterceptionLayout.SetScrollInterceptionListener(mInterceptionListener);
			mTitle = FindViewById<TextView>(Resource.Id.title);
			mTitle.Text=(Title);
			mTitle.TranslationY = (HeaderBarHeight - mActionBarSize) / 2;

			Title=(null);

			if (bundle == null) {
				mInitialTranslationY = FlexibleSpaceImageHeight - HeaderBarHeight;
			}

			ScrollUtils.addOnGlobalLayoutListener (InterceptionLayout, new Runnable (
				() => {
					mReady = true;
					UpdateViews (mInitialTranslationY, false);
				}
			));
		}

		private void changeHeaderBackgroundHeightAnimated(bool shouldShowGap, bool animated) {
			if (mGapIsChanging) {
				return;
			}
			int heightOnGapShown = mHeaderBar.Height;
			int heightOnGapHidden = mHeaderBar.Height + mActionBarSize;
			float from = mHeaderBackground.LayoutParameters.Height;
			float to;
			if (shouldShowGap) {
				if (!mGapHidden) {
					// Already shown
					return;
				}
				to = heightOnGapShown;
			} else {
				if (mGapHidden) {
					// Already hidden
					return;
				}
				to = heightOnGapHidden;
			}
			if (animated) {
				mHeaderBackground.Animate().Cancel();
				var a = ValueAnimator.OfFloat(from, to);
				a.SetDuration(100);
				a.SetInterpolator(new AccelerateDecelerateInterpolator());
				a.AddUpdateListener(new UpdateAnimatorListener(this, to, heightOnGapHidden));
				a.Start();
			} else {
				ChangeHeaderBackgroundHeight(to, to, heightOnGapHidden);
			}
		}

		public void ChangeHeaderBackgroundHeight(float height, float to, float heightOnGapHidden) {

			mHeaderBackground.LayoutParameters.Height = (int)height;
			(mHeaderBackground.LayoutParameters as Android.Widget.FrameLayout.LayoutParams)
				.TopMargin = (int)(mHeaderBar.Height - height);
			mHeaderBackground.RequestLayout ();
			mGapIsChanging = (height != to);
			if (!mGapIsChanging) {
				mGapHidden = (height == heightOnGapHidden);
			}
		}



		#region IObservableScrollViewCallbacks implementation
		public void OnScrollChanged (int scrollY, bool firstScroll, bool dragging)
		{
		}
		public void OnDownMotionEvent ()
		{
		}
		public void OnUpOrCancelMotionEvent (Xam.UI.ScrollState scrollState)
		{
		}
		#endregion



		internal class UpdateAnimatorListener: Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
		{
			IChangeBackgroundTarget _target;
			float _to, _heightOnGapHidden;

			public UpdateAnimatorListener (IChangeBackgroundTarget target, 
				float to, 
				float heightOnGapHidden)
			{
				_target = target;
				_to = to;
				_heightOnGapHidden = heightOnGapHidden;
			}

			public void OnAnimationUpdate (ValueAnimator animation)
			{
				float height = (float)animation.AnimatedValue;
				_target.ChangeHeaderBackgroundHeight (height, _to, _heightOnGapHidden);
			}
		}

		public class FillGapListener : ITouchInterceptionListener
		{
			IInterceptionTarget _target;

			internal FillGapListener (IInterceptionTarget target)
			{
				_target = target;
			}

			#region ITouchInterceptionListener implementation

			public bool ShouldInterceptTouchEvent (Android.Views.MotionEvent ev, bool moving, float diffX, float diffY)
			{
				return _target.MinInterceptionLayoutY < (int)_target.InterceptionLayout.GetY ()
					|| (moving && _target.Scrollable.CurrentScrollY - diffY < 0);
			}

			public void OnDownMotionEvent (Android.Views.MotionEvent ev)
			{
				_target.ScrollYOnDownMotion = _target.Scrollable.CurrentScrollY;
			}

			public void OnMoveMotionEvent (Android.Views.MotionEvent ev, float diffX, float diffY)
			{
				float translationY = _target.InterceptionLayout.TranslationY - _target.ScrollYOnDownMotion + diffY;
				float minTranslationY = _target.MinInterceptionLayoutY;
				if (translationY < minTranslationY) {
					translationY = minTranslationY;
				} else if (_target.FlexibleSpaceImageHeight - _target.HeaderBarHeight < translationY) {
					translationY = _target.FlexibleSpaceImageHeight - _target.HeaderBarHeight;
				}

				_target.UpdateViews (translationY, true);
			}

			public void OnUpOrCancelMotionEvent (Android.Views.MotionEvent ev)
			{

			}

			#endregion	
		}
	}


}

