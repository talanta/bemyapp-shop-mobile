using System;

namespace weshop.portable
{
	public interface IDialogService
	{
		void ShowAbout();

		void ShowProgress();

		void Dismiss();

		void ToastSuccess(string message, int timeout = 2000);

		void ToastError (string message, int timeout = 2000);

		void ShowProduct(string url);
	}
}

