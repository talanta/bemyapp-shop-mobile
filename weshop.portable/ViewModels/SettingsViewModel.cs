using System;

namespace weshop.portable
{
	public class SettingsViewModel: BaseViewModel
	{
		public const string TYPENAME = "SettingsViewModel";

		public SettingsViewModel ()
		{

		}

		public void Close()
		{
			this.Close (this);
		}
	}
}

