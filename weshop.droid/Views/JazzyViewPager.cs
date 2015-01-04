using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;
using Android.Support.V4.View;

namespace weshop.droid
{
	public enum TransitionEffect
	{
		Standard,
		Tablet,
		CubeIn,
		CubeOut,
		FlipVertical,
		FlipHorizontal,
		Stack,
		ZoomIn,
		ZoomOut,
		RotateUp,
		RotateDown,
		Accordion
	}

	public class JazzyViewPager: Android.Support.V4.View.ViewPager
	{

		public const String TAG = "JazzyViewPager";

		private bool mEnabled = true;
		private bool mOutlineEnabled = false;
		public static Color sOutlineColor = Android.Graphics.Color.White;
		private TransitionEffect mEffect = TransitionEffect.Standard;

		private Dictionary<int, Java.Lang.Object> mObjs = new Dictionary<int, Java.Lang.Object> ();

		private const  float SCALE_MAX = 0.5f;
		private const  float ZOOM_MAX = 0.5f;
		private const  float ROT_MAX = 15.0f;


		public JazzyViewPager (Context context) : this (context, null)
		{
		
		}
			
		public JazzyViewPager (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{	
			SetClipChildren (false);
			// now style everything!
			var ta = context.ObtainStyledAttributes (attrs, Resource.Styleable.JazzyViewPager);
			int effect = ta.GetInt (Resource.Styleable.JazzyViewPager_style, 0);
			String[] transitions = Resources.GetStringArray (Resource.Array.jazzy_effects);
			setTransitionEffect ((TransitionEffect)Enum.Parse (typeof(TransitionEffect), transitions [effect]));

			FadeEnabled = (ta.GetBoolean (Resource.Styleable.JazzyViewPager_fadeEnabled, false));
			setOutlineEnabled (ta.GetBoolean (Resource.Styleable.JazzyViewPager_outlineEnabled, false));
			sOutlineColor = (ta.GetColor (Resource.Styleable.JazzyViewPager_outlineColor, Android.Graphics.Color.White));

			switch (mEffect) {
			case  TransitionEffect.Stack:
			case TransitionEffect.ZoomOut:
				FadeEnabled = true;
				break;
			default:
				break;
			}
			ta.Recycle ();
		}

		public void setTransitionEffect (TransitionEffect effect)
		{
			mEffect = effect;
		}

		public bool PagingEnabled{ get; private set; }

		public bool FadeEnabled  { get; private set; }


		public void setOutlineEnabled (bool enabled)
		{
			mOutlineEnabled = enabled;
			wrapWithOutlines ();
		}

		//		public void setOutlineColor(Color color) {
		//			sOutlineColor = color;
		//		}

		private void wrapWithOutlines ()
		{		
			for (int i = 0; i < ChildCount; i++) {
				View v = GetChildAt (i);
				if (!(v is OutlineContainer)) {
					RemoveView (v);
					base.AddView (wrapChild (v), i);

				}
			}
		}

		private View wrapChild (View child)
		{
			if (!mOutlineEnabled || child is OutlineContainer)
				return child;
			OutlineContainer outl = new OutlineContainer (Context);
			outl.LayoutParameters = GenerateDefaultLayoutParams ();

			child.LayoutParameters = (new OutlineContainer.LayoutParams (
				OutlineContainer.LayoutParams.MatchParent, OutlineContainer.LayoutParams.MatchParent));
			outl.AddView (child);
			return outl;
		}

		public void addView (View child)
		{
			base.AddView (wrapChild (child));
		}

		public void addView (View child, int index)
		{
			base.AddView (wrapChild (child), index);
		}

		public void addView (View child, LayoutParams p)
		{
			base.AddView (wrapChild (child), p);
		}

		public void addView (View child, int width, int height)
		{
			base.AddView (wrapChild (child), width, height);
		}

		public void addView (View child, int index, LayoutParams p)
		{
			base.AddView (wrapChild (child), index, p);
		}


		public override bool OnInterceptTouchEvent (MotionEvent arg0)
		{
			return mEnabled ? base.OnInterceptTouchEvent (arg0) : false;
		}

		private State mState;
		private int oldPage;

		private View mLeft;
		private View mRight;
		private float mRot;
		private float mTrans;
		private float mScale;

		private enum State
		{
			IDLE,
			GOING_LEFT,
			GOING_RIGHT
		}

		//	public void reset() {
		//	resetPrivate();
		//	int curr = getCurrentItem();
		//	onPageScrolled(curr, 0.0f, 0);
		//}
		//
		//private void resetPrivate() {
		//	for (int i = 0; i < getChildCount(); i++) {
		//		View v = getChildAt(i);
		//		//			ViewHelper.setRotation(v, -ViewHelper.getRotation(v));
		//		//			ViewHelper.setRotationX(v, -ViewHelper.getRotationX(v));
		//		//			ViewHelper.setRotationY(v, -ViewHelper.getRotationY(v));
		//		//
		//		//			ViewHelper.setTranslationX(v, -ViewHelper.getTranslationX(v));
		//		//			ViewHelper.setTranslationY(v, -ViewHelper.getTranslationY(v));
		//
		//		ViewHelper.setRotation(v, 0);
		//		ViewHelper.setRotationX(v, 0);
		//		ViewHelper.setRotationY(v, 0);
		//
		//		ViewHelper.setTranslationX(v, 0);
		//		ViewHelper.setTranslationY(v, 0);
		//
		//		ViewHelper.setAlpha(v, 1.0f);
		//
		//		ViewHelper.setScaleX(v, 1.0f);
		//		ViewHelper.setScaleY(v, 1.0f);
		//
		//		ViewHelper.setPivotX(v, 0);
		//		ViewHelper.setPivotY(v, 0);
		//
		//		logState(v, "Child " + i);
		//	}
		//}

		private void logState (View v, String title)
		{
			Log.Verbose (TAG, title + ": ROT (" + v.Rotation + ", " +
			v.RotationX + ", " +
			v.RotationY + "), TRANS (" +
			v.TranslationX + ", " +
			v.TranslationY + "), SCALE (" +
			v.ScaleX + ", " +
			v.ScaleY + "), ALPHA " +
			v.Alpha);
		}

		protected void animateScroll (int position, float positionOffset)
		{
			if (mState != State.IDLE) {
				mRot = (float)(1 - Math.Cos (2 * Math.PI * positionOffset)) / 2 * 30.0f;
				this.RotationY = mState == State.GOING_RIGHT ? mRot : -mRot;
				this.PivotX = MeasuredWidth * 0.5f;
				this.PivotY = MeasuredHeight * 0.5f;
			}
		}

		protected void animateTablet (View left, View right, float positionOffset)
		{		
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					mRot = 30.0f * positionOffset;
					mTrans = getOffsetXForRotation (mRot, left.MeasuredWidth, left.MeasuredHeight);
					left.PivotX = left.MeasuredWidth / 2;
					left.PivotY = left.MeasuredHeight / 2;
					left.TranslationX = mTrans;
					left.RotationY = mRot;
					logState (left, "Left");
				}
				if (right != null) {
					manageLayer (right, true);
					mRot = -30.0f * (1 - positionOffset);
					mTrans = getOffsetXForRotation (mRot, right.MeasuredWidth, 
						right.MeasuredHeight);
					right.PivotX = right.MeasuredWidth * 0.5f;
					right.PivotY = right.MeasuredHeight * 0.5f;
					right.TranslationX = mTrans;
					right.RotationY = mRot;
					logState (right, "Right");
				}
			}
		}

		private void animateCube (View left, View right, float positionOffset, bool isIn)
		{
			if (mState != State.IDLE) {
				if (left != null) {

					manageLayer (left, true);
					mRot = (isIn ? 90.0f : -90.0f) * positionOffset;
					left.PivotX = left.MeasuredWidth;
					left.PivotY = left.MeasuredHeight * 0.5f;
					left.RotationY = mRot;
				}
				if (right != null) {
					manageLayer (right, true);
					mRot = -(isIn ? 90.0f : -90.0f) * (1 - positionOffset);
					right.PivotX = 0;
					right.PivotY = right.MeasuredHeight * 0.5f;
					right.RotationY = mRot;
				}
			}
		}

		private void animateAccordion (View left, View right, float positionOffset)
		{
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					left.PivotX = left.MeasuredWidth;
					left.PivotY = 0;
					left.ScaleX = 1 - positionOffset;
				}
				if (right != null) {
					manageLayer (right, true);
					right.PivotX = 0;
					right.PivotY = 0;
					right.ScaleX = positionOffset;
				}
			}
		}

		private void animateZoom (View left, View right, float positionOffset, bool isIn)
		{
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					mScale = isIn ? ZOOM_MAX + (1 - ZOOM_MAX) * (1 - positionOffset) :
						1 + ZOOM_MAX - ZOOM_MAX * (1 - positionOffset);

					left.PivotX = left.MeasuredWidth * 0.5f;
					left.PivotY = left.MeasuredHeight * 0.5f;
					left.ScaleX = mScale;
					left.ScaleY = mScale;
				}
				if (right != null) {
					manageLayer (right, true);
					mScale = isIn ? ZOOM_MAX + (1 - ZOOM_MAX) * positionOffset :
						1 + ZOOM_MAX - ZOOM_MAX * positionOffset;


					right.PivotX = right.MeasuredWidth * 0.5f;
					right.PivotY = right.MeasuredHeight * 0.5f;
					right.ScaleX = mScale;
					right.ScaleY = mScale;
				}
			}
		}

		private void animateRotate (View left, View right, float positionOffset, bool up)
		{
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					mRot = (up ? 1 : -1) * (ROT_MAX * positionOffset);
					mTrans = (up ? -1 : 1) * (float)(MeasuredHeight - MeasuredHeight * Math.Cos (mRot * Math.PI / 180.0f));
					left.PivotX = left.MeasuredWidth * 0.5f;
					left.PivotY = up ? 0 : left.MeasuredHeight;
					left.TranslationY = mTrans;
					left.Rotation = mRot;
				}
				if (right != null) {
					manageLayer (right, true);
					mRot = (up ? 1 : -1) * (-ROT_MAX + ROT_MAX * positionOffset);
					mTrans = (up ? -1 : 1) * (float)(MeasuredHeight - MeasuredHeight * Math.Cos (mRot * Math.PI / 180.0f));
					right.PivotX = right.MeasuredWidth * 0.5f;
					right.PivotY = up ? 0 : right.MeasuredHeight;
					right.TranslationY = mTrans;
					right.Rotation = mRot;
				}
			}
		}

		private void animateFlipHorizontal (View left, View right, float positionOffset, int positionOffsetPixels)
		{
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					mRot = 180.0f * positionOffset;
					if (mRot > 90.0f) {
						left.Visibility = ViewStates.Invisible;
					} else {
						if (left.Visibility == ViewStates.Invisible)
							left.Visibility = (ViewStates.Visible);
						mTrans = positionOffsetPixels;
						left.PivotX = left.MeasuredWidth * 0.5f;
						left.PivotY = left.MeasuredHeight * 0.5f;
						left.TranslationX = mTrans;
						left.RotationY = mRot;
					}
				}
				if (right != null) {
					manageLayer (right, true);
					mRot = -180.0f * (1 - positionOffset);
					if (mRot < -90.0f) {
						right.Visibility = (ViewStates.Invisible);
					} else {
						if (right.Visibility == ViewStates.Invisible)
							right.Visibility = ViewStates.Visible;
						mTrans = -Width - PageMargin + positionOffsetPixels;
						right.PivotX = right.MeasuredWidth * 0.5f;
						right.PivotY = right.MeasuredHeight * 0.5f;
						right.TranslationX = mTrans;
						right.RotationY = mRot;
					}
				}
			}
		}

		private void animateFlipVertical (View left, View right, float positionOffset, int positionOffsetPixels)
		{
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					mRot = 180.0f * positionOffset;
					if (mRot > 90.0f) {
						left.Visibility = ViewStates.Invisible;
					} else {
						if (left.Visibility == ViewStates.Invisible)
							left.Visibility = ViewStates.Visible;
						mTrans = positionOffsetPixels;
						left.PivotX = left.MeasuredWidth * 0.5f;
						left.PivotY = left.MeasuredHeight * 0.5f;
						left.TranslationX = mTrans;
						left.RotationX = mRot;
					}
				}
				if (right != null) {
					manageLayer (right, true);
					mRot = -180.0f * (1 - positionOffset);
					if (mRot < -90.0f) {
						right.Visibility = ViewStates.Invisible;
					} else {
						if (right.Visibility == ViewStates.Invisible)
							right.Visibility = ViewStates.Visible;
						mTrans = -Width - PageMargin + positionOffsetPixels;
						right.PivotX = right.MeasuredWidth * 0.5f;
						right.PivotY = right.MeasuredHeight * 0.5f;
						right.TranslationX = mTrans;
						right.RotationX = mRot;
					}
				}
			}
		}

		protected void animateStack (View left, View right, float positionOffset, int positionOffsetPixels, int position)
		{		
			View view2 = findViewFromObject (position + 2);

			if (view2 != null) {

				view2.Visibility = (mState == State.GOING_LEFT) ? ViewStates.Invisible : ViewStates.Visible;
			}
			var view3 = findViewFromObject (position + 1);
			if (view3 != null) {
				view3.Visibility = ViewStates.Visible;
			}

			if (mState != State.IDLE) {
				if (right != null) {
					manageLayer (right, true);
					mScale = (1 - SCALE_MAX) * positionOffset + SCALE_MAX;
					mTrans = -Width - PageMargin + positionOffsetPixels;

					//right.ScaleX = mScale;
					//right.ScaleY = mScale;
					right.TranslationX = mTrans;
				}
				if (left != null) {
					left.BringToFront ();
					manageLayer (left, true);
					mRot = -(ROT_MAX * positionOffset);
					left.Rotation = mRot;
				}
			}
		}

		private void manageLayer (View v, bool enableHardware)
		{
			if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Honeycomb)
				return;
			var layerType = enableHardware ? Android.Views.LayerType.Hardware : Android.Views.LayerType.None;
			if (layerType != v.LayerType)
				v.SetLayerType (layerType, null);
		}


		private void disableHardwareLayer ()
		{
			if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Honeycomb)
				return;
			View v;
			for (int i = 0; i < ChildCount; i++) {
				v = GetChildAt (i);
				if (v.LayerType != LayerType.None)
					v.SetLayerType (LayerType.None, null);
			}
		}

		private Matrix mMatrix = new Matrix ();
		private Camera mCamera = new Camera ();
		private float[] mTempFloat2 = new float[2];

		protected float getOffsetXForRotation (float degrees, int width, int height)
		{
			mMatrix.Reset ();
			mCamera.Save ();
			mCamera.RotateY (Math.Abs (degrees));
			mCamera.GetMatrix (mMatrix);
			mCamera.Restore ();

			mMatrix.PreTranslate (-width * 0.5f, -height * 0.5f);
			mMatrix.PostTranslate (width * 0.5f, height * 0.5f);
			mTempFloat2 [0] = width;
			mTempFloat2 [1] = height;
			mMatrix.MapPoints (mTempFloat2);
			return (width - mTempFloat2 [0]) * (degrees > 0.0f ? 1.0f : -1.0f);
		}

		protected void animateFade (View left, View right, float positionOffset)
		{
			if (left != null) {
				left.Alpha = 1 - positionOffset;
			}
			if (right != null) {
				right.Alpha = positionOffset;
			}
		}

		protected void animateOutline (View left, View right)
		{
			if (!(left is OutlineContainer))
				return;
			if (mState != State.IDLE) {
				if (left != null) {
					manageLayer (left, true);
					((OutlineContainer)left).setOutlineAlpha (1.0f);
				}
				if (right != null) {
					manageLayer (right, true);
					((OutlineContainer)right).setOutlineAlpha (1.0f);
				}
			} else {
				if (left != null)
					((OutlineContainer)left).Start ();
				if (right != null)
					((OutlineContainer)right).Start ();
			}
		}

		//@Override
		protected override void OnPageScrolled (int position, float positionOffset, int positionOffsetPixels)
		{
			if (mState == State.IDLE && positionOffset > 0) {

				oldPage = CurrentItem;
				mState = position == oldPage ? State.GOING_RIGHT : State.GOING_LEFT;
			}
			bool goingRight = position == oldPage;				
			if (mState == State.GOING_RIGHT && !goingRight)
				mState = State.GOING_LEFT;
			else if (mState == State.GOING_LEFT && goingRight)
				mState = State.GOING_RIGHT;

			float effectOffset = isSmall (positionOffset) ? 0 : positionOffset;

			//		mLeft = getChildAt(position);
			//		mRight = getChildAt(position+1);
			mLeft = findViewFromObject (position);
			mRight = findViewFromObject (position + 1);

			if (FadeEnabled)
				animateFade (mLeft, mRight, effectOffset);
			if (mOutlineEnabled)
				animateOutline (mLeft, mRight);

			switch (mEffect) {
			case TransitionEffect.Standard:
				break;
			case TransitionEffect.Tablet:
				animateTablet (mLeft, mRight, effectOffset);
				break;
			case TransitionEffect.CubeIn:
				animateCube (mLeft, mRight, effectOffset, true);
				break;
			case TransitionEffect.CubeOut:
				animateCube (mLeft, mRight, effectOffset, false);
				break;
			case TransitionEffect.FlipVertical:
				animateFlipVertical (mLeft, mRight, positionOffset, positionOffsetPixels);
				break;
			case TransitionEffect.FlipHorizontal:
				animateFlipHorizontal (mLeft, mRight, effectOffset, positionOffsetPixels);
				break;
			case TransitionEffect.Stack:
				animateStack (mLeft, mRight, effectOffset, positionOffsetPixels, position);
				break;
			case TransitionEffect.ZoomIn:
				animateZoom (mLeft, mRight, effectOffset, true);
				break;
			case TransitionEffect.ZoomOut:
				animateZoom (mLeft, mRight, effectOffset, false);
				break;
			case TransitionEffect.RotateUp:
				animateRotate (mLeft, mRight, effectOffset, true);
				break;
			case TransitionEffect.RotateDown:
				animateRotate (mLeft, mRight, effectOffset, false);
				break;
			case TransitionEffect.Accordion:
				animateAccordion (mLeft, mRight, effectOffset);
				break;
			}

			base.OnPageScrolled (position, positionOffset, positionOffsetPixels);

			if (effectOffset == 0) {
				disableHardwareLayer ();
				mState = State.IDLE;
			}

		}

		private bool isSmall (float positionOffset)
		{
			return Math.Abs (positionOffset) < 0.0001;
		}

		public void setObjectForPosition (Java.Lang.Object obj, int position)
		{
			if (mObjs.ContainsKey (position))
				mObjs [position] = obj;
			else
				mObjs.Add (position, obj);
		}

		public View findViewFromObject (int position)
		{
			Java.Lang.Object o;

			if (!mObjs.TryGetValue (position, out o) || o == null) {
				return null;
			}
			View v;
			for (int i = 0; i < ChildCount; i++) {
				v = GetChildAt (i);
				if (Adapter.IsViewFromObject (v, o))
					return v;
			}
			return null;
		}

	}
}

