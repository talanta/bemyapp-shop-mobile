using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace weshop.portable.ViewModels
{
	public class FirstViewModel 
		: MvxViewModel
	{
		private MvxCommand<MenuViewModel> selectMenuItemCommand;
		private List<MenuViewModel> menuItems;
		private MainViewModel _mainViewModel;
		private WishsetViewModel _wishSetViewModel;
		private WishlistViewModel _wishlistViewModel;

		public FirstViewModel ()
		{
			this.menuItems = new List<MenuViewModel> {
				new MenuViewModel{ Section = typeof(MainViewModel), Title = "Séduction" },
				new MenuViewModel{ Section = typeof(WishsetViewModel), Title = "Catégories" },
				new MenuViewModel{ Section = typeof(WishlistViewModel), Title = "Mon jardin secret" },
			};
						
		}

		public WishlistViewModel WishlistViewModel{ get { return _wishlistViewModel ?? (_wishlistViewModel = new WishlistViewModel ()); } }
		public WishsetViewModel WishSetViewModel{ get { return _wishSetViewModel ?? (_wishSetViewModel = new WishsetViewModel ()); } }
		public MainViewModel MainViewModel{ get { return _mainViewModel ?? (_mainViewModel = new MainViewModel ()); } }

		public List<MenuViewModel> MenuItems{ get { return this.menuItems; } }

		public ICommand SelectMenuItemCommand {
			get {
				return this.selectMenuItemCommand ?? (this.selectMenuItemCommand = new MvxCommand<MenuViewModel> (x => {   
					this.ShowViewModel (x.Section);                    
				}));
			}
		}

	}
}
