using System;
using Android.Content;
using Android.Util;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Attributes;
using System.Collections;
using System.Windows.Input;
using Android.Views;

namespace weshop.droid
{
	public class BindableViewPager
		: JazzyViewPager
	{
		public BindableViewPager(Context context, IAttributeSet attrs)
			: this(context, attrs, new MvxBindablePagerAdapter(context))
		{ }

		public BindableViewPager(Context context, IAttributeSet attrs, MvxBindablePagerAdapter adapter)
			: base(context, attrs)
		{
			var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
			adapter.ItemTemplateId = itemTemplateId;
			Adapter = adapter;
			this.PageMargin = 30;
		}

		public new MvxBindablePagerAdapter Adapter
		{
			get { return base.Adapter as MvxBindablePagerAdapter; }
			set
			{
				var existing = Adapter;
				if (existing == value)
					return;

				if (existing != null && value != null)
				{
					value.ItemsSource = existing.ItemsSource;
					value.ItemTemplateId = existing.ItemTemplateId;
				}

				base.Adapter = value;
			}
		}

		[MvxSetToNullAfterBinding]
		public IEnumerable ItemsSource
		{
			get { return Adapter.ItemsSource; }
			set { Adapter.ItemsSource = value; }
		}

		public int ItemTemplateId
		{
			get { return Adapter.ItemTemplateId; }
			set { Adapter.ItemTemplateId = value; }
		}

		private ICommand _itemPageSelected;
		public ICommand ItemPageSelected
		{
			get { return _itemPageSelected; }
			set { _itemPageSelected = value; if (_itemPageSelected != null) EnsureItemPageSelectedOverloaded(); }
		}

		private bool _itemPageSelectedOverloaded;
		private void EnsureItemPageSelectedOverloaded()
		{
			if (_itemPageSelectedOverloaded)
				return;

			_itemPageSelectedOverloaded = true;
			base.PageSelected += (sender, args) => ExecuteCommandOnItem(ItemPageSelected, args.Position);
		}

		protected virtual void ExecuteCommandOnItem(ICommand command, int position)
		{
			if (command == null)
				return;

			var item = Adapter.GetRawItem(position);
			if (item == null)
				return;

			if (!command.CanExecute(item))
				return;

			command.Execute(item);
		}

		private ICommand _pageSelected;
		public new ICommand PageSelected
		{
			get { return _pageSelected; }
			set { _pageSelected = value; if (_pageSelected != null) EnsurePageSelectedOverloaded(); }
		}

		private bool _pageSelectedOverloaded;
		private void EnsurePageSelectedOverloaded()
		{
			if (_pageSelectedOverloaded)
				return;

			_pageSelectedOverloaded = true;
			base.PageSelected += (sender, args) => ExecuteCommand(PageSelected, args.Position);
		}

		protected virtual void ExecuteCommand(ICommand command, int toPage)
		{
			if (command == null)
				return;

			if (!command.CanExecute(toPage))
				return;

			command.Execute(toPage);
		}

	
	} 
}

