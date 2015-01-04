using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views.Animations;
using Android.Widget;
using Android.Content.Res;

namespace weshop.droid
{


	public class OutlineInterpolator : Java.Lang.Object, ITimeInterpolator
	{
		public float GetInterpolation (float t)
		{
			t -= 1.0f;
			return t * t * t + 1.0f;
		}
	}

	public class OutlineContainer : FrameLayout, IAnimatable
	{
		private Paint mOutlinePaint;

		private bool mIsRunning = false;
		private long mStartTime;
		private float mAlpha = 1.0f;
		private const long ANIMATION_DURATION = 500;
		private const long FRAME_DURATION = 1000 / 60;
		Action mUpdater; 
		private ITimeInterpolator mInterpolator = new OutlineInterpolator ();

		public OutlineContainer (Context context) : base (context)
		{
			init ();
		}

		public OutlineContainer (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{
			init ();
		}

		public OutlineContainer (Context context, IAttributeSet attrs, int defStyle)
			: base (context, attrs, defStyle)
		{
			init ();
		}

		internal static int dpToPx(Resources res, int dp) {
			return (int)(dp * res.DisplayMetrics.Density); // margin in pixels
			//return (int) TypedValue.ApplyDimension(TypedValue., dp, res.getDisplayMetrics());
		}

		private void init ()
		{
			mUpdater = () =>
			{
				long now = AnimationUtils.CurrentAnimationTimeMillis ();
				long duration = now - mStartTime;
				if (duration >= ANIMATION_DURATION) {
					mAlpha = 0.0f;
					Invalidate ();
					Stop ();
					return;
				} else {
					mAlpha = mInterpolator.GetInterpolation (1 - duration / (float)ANIMATION_DURATION);
					Invalidate ();
				}
				PostDelayed (mUpdater, FRAME_DURATION);
			};

			mOutlinePaint = new Paint ();
			mOutlinePaint.AntiAlias = (true);
		

			mOutlinePaint.StrokeWidth = (dpToPx (Resources, 2));
			mOutlinePaint.Color = (Resources.GetColor (Resource.Color.refractored_holo_blue_dark));
			mOutlinePaint.SetStyle(Android.Graphics.Paint.Style.Stroke);

			int padding = dpToPx (Resources, 10);
			SetPadding (padding, padding, padding, padding);
		}

		//@Override
		protected override void DispatchDraw (Canvas canvas)
		{
			base.DispatchDraw (canvas);
			int offset = dpToPx (Resources, 5);
			if (mOutlinePaint.Color != JazzyViewPager.sOutlineColor) {
				mOutlinePaint.Color = (JazzyViewPager.sOutlineColor);
			}
			mOutlinePaint.Alpha = ((int)(mAlpha * 255));
			Rect rect = new Rect (offset, offset, MeasuredWidth - offset, MeasuredHeight - offset);
			canvas.DrawRect (rect, mOutlinePaint);
		}

		public void setOutlineAlpha (float alpha)
		{
			mAlpha = alpha;
		
		}
				
		//@Override
		public  bool IsRunning { get { return mIsRunning; } }


		//@Override
		public void Start ()
		{
			if (mIsRunning)
				return;
			mIsRunning = true;
			mStartTime = AnimationUtils.CurrentAnimationTimeMillis ();	
			Post (mUpdater);
		}

		//	@Override
		public void Stop ()
		{
			if (!mIsRunning)
				return;
			mIsRunning = false;
		}

	}
}

