using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Linq;

namespace weshop.portable
{
	public class WishlistService : IWishListService
	{
		readonly ISQLiteConnection _connection;

		#region IWishListService implementation

		public void AddItem (Product item)
		{
			var existing = _connection.Table<Product> ().Any (p => p.Id == item.Id);
			if (existing)
				return;

			item.BestOffer.ProductId = item.Id;
			_connection.Insert (item.BestOffer);
			_connection.Insert (item);
		}

		public System.Collections.Generic.IList<Product> GetAllProducts ()
		{
			var productList = _connection.Table<Product> ().ToList ();
		
			foreach (var product in productList) {
				product.BestOffer = _connection.Table<BestOffer> ().FirstOrDefault (b => b.ProductId == product.Id);
			}

			return productList;
		}

		#endregion

		public WishlistService ()
		{
			var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
			// open or create the database
			_connection = factory.Create("mydb.sql");
			// ensure our tables exist
			_connection.CreateTable<BestOffer> ();
			_connection.CreateTable<Product>();

		}
	}
}

