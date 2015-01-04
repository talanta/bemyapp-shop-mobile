using System;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Android.Content;
using Android.Util;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Android.Graphics;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;
using Cirrious.CrossCore.Core;
using Android.Widget;
using Android.Support.V7.Graphics;

namespace weshop.droid
{
	public class PaletteImageView
		: ImageView
	{
		private readonly IMvxImageHelper<Bitmap> _imageHelper;

		public PaletteImageView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			if (!Mvx.TryResolve(out _imageHelper))
			{
				MvxTrace.Error(
					"No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
			}
			else
			{
				_imageHelper.ImageChanged += ImageHelperOnImageChanged;
			}

			var typedArray = context.ObtainStyledAttributes(attrs,
				MvxAndroidBindingResource.Instance
				.ImageViewStylableGroupId);

			int numStyles = typedArray.IndexCount;
			for (var i = 0; i < numStyles; ++i)
			{
				int attributeId = typedArray.GetIndex(i);
				if (attributeId == MvxAndroidBindingResource.Instance.SourceBindId)
				{
					ImageUrl = typedArray.GetString(attributeId);
				}
			}
			typedArray.Recycle();
		}

		public PaletteImageView(Context context)
			: base(context)
		{
			if (!Mvx.TryResolve(out _imageHelper))
			{
				MvxTrace.Error(
					"No IMvxImageHelper registered - you must provide an image helper before you can use a MvxImageView");
			}
			else
			{
				_imageHelper.ImageChanged += ImageHelperOnImageChanged;
			}
		}

		public string ImageUrl
		{
			get
			{
				if (_imageHelper == null)
					return null;
				return _imageHelper.ImageUrl;
			}
			set
			{
				if (_imageHelper == null)
					return;
				_imageHelper.ImageUrl = value;
			}
		}

		public string DefaultImagePath
		{
			get { return _imageHelper.DefaultImagePath; }
			set { _imageHelper.DefaultImagePath = value; }
		}

		public string ErrorImagePath
		{
			get { return _imageHelper.ErrorImagePath; }
			set { _imageHelper.ErrorImagePath = value; }
		}

	

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_imageHelper != null)
					_imageHelper.Dispose();
			}

			base.Dispose(disposing);
		}

		private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Bitmap> mvxValueEventArgs)
		{
//			if (mvxValueEventArgs.Value != null) {
//				Palette palette = Palette.Generate (mvxValueEventArgs.Value);
//				var test = palette.GetLightMutedColor (0);
//
//				SetColorFilter (new Color (test){ A = 150});
//			}
			SetImageBitmap(mvxValueEventArgs.Value);
		}
	}
}

