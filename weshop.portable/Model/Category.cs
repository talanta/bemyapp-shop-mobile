using System;
using System.ComponentModel;

namespace weshop.portable
{
	public class Category: INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyChange(string property)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged (this, new PropertyChangedEventArgs (property));
		}
		#endregion

		private bool _isVisible;

		public string Display { get; set; }
		public string Keyword { get; set; }
		public string ImageUri { get; set; }

		public bool IsVisible 
		{
			get{ return _isVisible; }
			set
			{
				_isVisible = value;
				NotifyChange ("IsVisible");
			}
		}
	}
}

