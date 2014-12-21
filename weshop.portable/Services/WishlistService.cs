﻿using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using System.Linq;
using System.Collections.Generic;

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

		public void RemoveItem(Product p)
		{
			var item  = _connection.Table<Product> ().FirstOrDefault (i => i.Id == p.Id);
			if (null == item)
				return;
			_connection.Delete (item);	
		}

		public System.Collections.Generic.IList<Product> GetAllProducts ()
		{
			var productList = _connection.Table<Product> ().ToList ();
		
			foreach (var product in productList) {
				product.BestOffer = _connection.Table<BestOffer> ().FirstOrDefault (b => b.ProductId == product.Id);
			}

			return productList;
		}

		public void FillLikes(IList<Product> products)
		{
			foreach (var p in products) {
				var liked = _connection.Table<Product> ().FirstOrDefault (b => b.Id == p.Id);
				if (null == liked)
					continue;
				p.Like = liked.Like;
			}
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

