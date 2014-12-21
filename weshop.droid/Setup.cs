using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using weshop.portable;
using Cirrious.MvvmCross.Droid.Views;
using System.Reflection;
using System.Collections.Generic;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace weshop.droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			return new portable.App();
        }

		protected override void FillValueConverters (IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters (registry);
			registry.AddOrOverwrite ("MenuIcon", new MenuIconConverter ());
		}

		protected override System.Collections.Generic.List<System.Reflection.Assembly> ValueConverterAssemblies 
		{
			get 
			{
				var toReturn = base.ValueConverterAssemblies;
				toReturn.Add(typeof(MvxVisibilityValueConverter).Assembly);
				return toReturn;
			}
		} 

		//override 

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var customPresenter = new CustomPresenter();
			Mvx.RegisterSingleton<ICustomPresenter>(customPresenter);
			return customPresenter;
		}

		protected override void InitializeLastChance ()
		{
			base.InitializeLastChance ();
			Mvx.RegisterType<IDialogService, DialogService> ();
		}

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}