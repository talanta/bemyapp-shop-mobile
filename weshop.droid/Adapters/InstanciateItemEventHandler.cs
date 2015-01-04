using System;
using Android.Views;

namespace weshop.droid
{
	public delegate void InstanciateItemEventHandler(object sender, InstanciateItemEventArgs args);
	public delegate void DestroyItemEventHandler(object sender, DestroyItemEventArgs args);

	public class DestroyItemEventArgs
	{
		readonly ViewGroup _viewGroup;
		readonly int _position;

		public DestroyItemEventArgs (ViewGroup container, int position)
		{
			this._position = position;
			this._viewGroup = container;
		}
		public int Position {get{ return _position; }}
		public ViewGroup ViewGroup {get{ return _viewGroup; }}
	}

	public class InstanciateItemEventArgs
	{
		readonly View _view;
		readonly int _position;

		public InstanciateItemEventArgs (View view, int position)
		{
			this._view = view;
			this._position = position;
		}

		public View View {get{ return _view; }}
		public int Position {get{ return _position; }}
	}
}

