using System;

namespace weshop.portable
{
	public interface IDialogService
	{
		void ShowProgress();

		void Dismiss();

		void ToastSuccess(string message, int timeout = 2000);
	}
}

