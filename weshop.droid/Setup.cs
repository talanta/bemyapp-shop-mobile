using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using weshop.portable;

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