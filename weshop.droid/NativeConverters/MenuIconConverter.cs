using System;
using Cirrious.CrossCore.Converters;
using weshop.portable;
using weshop.portable.ViewModels;
using System.Globalization;

namespace weshop.droid
{
	public class MenuIconConverter
		: MvxValueConverter<Type, string>
	{
		protected override string Convert (Type value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value.Name) {
			case MainViewModel.TYPENAME:
				return "res:ic_home_white_36dp";
			case WishsetViewModel.TYPENAME:
				return "res:ic_dashboard_white_36dp";
			case WishlistViewModel.TYPENAME:
				return "res:ic_loyalty_white_36dp";
			case SettingsViewModel.TYPENAME:
				return "res:ic_settings_applications_white_36dp";
			default:
				break;
			}
			return "res:ic_help_white_36dp";
		}


	}
}

