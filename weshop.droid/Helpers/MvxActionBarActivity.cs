/*
 * MeetupManager:
 * Copyright (C) 2013 Refractored LLC: 
 * http://github.com/JamesMontemagno
 * http://twitter.com/JamesMontemagno
 * http://refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace weshop.droid.Helpers
{
	public abstract class MvxActionBarActivity
		: MvxActionBarEventSourceActivity
	, IMvxAndroidView
	{
		protected MvxActionBarActivity ()
		{
			BindingContext = new MvxAndroidBindingContext (this, this);
			this.AddEventListeners ();
		}

		public Toolbar Toolbar { get; protected set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (LayoutResource);
			Toolbar = FindViewById<Toolbar> (ToolbarResourceId);
			if (Toolbar != null) {
				SetSupportActionBar (Toolbar);
			}
		}

		protected abstract int ToolbarResourceId { get; }

		protected abstract int LayoutResource{ get; }

		public IMvxBindingContext BindingContext { get; set; }

		public object DataContext {
			get { return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}

		public IMvxViewModel ViewModel {
			get { return DataContext as IMvxViewModel; }
			set {
				DataContext = value;
				OnViewModelSet ();
			}
		}

		public void MvxInternalStartActivityForResult (Intent intent, int requestCode)
		{
			base.StartActivityForResult (intent, requestCode);
		}

		public override void SetContentView (int layoutResId)
		{
			var view = this.BindingInflate (layoutResId, null);
			SetContentView (view);
		}

		protected virtual void OnViewModelSet ()
		{
		}
	}
}