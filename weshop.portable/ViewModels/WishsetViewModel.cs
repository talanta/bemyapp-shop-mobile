using System;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;

namespace weshop.portable.ViewModels
{
	public class WishsetViewModel
		: MvxViewModel
	{
		public const string TYPENAME = "WishsetViewModel";

		public readonly List<Category> _categories = new List<Category> ();

		public List<Category> Categories { get { return _categories; } }

		public WishsetViewModel ()
		{
			var cat = Newtonsoft.Json.JsonConvert.DeserializeObject<Category[]> (weshop.portable.Categories.JSON);
			this.Categories.AddRange (cat);
		}

		protected override void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);
		}

		public void GoToMeet()
		{
			var bundle = new MvxBundle ();

			bundle.Data.Add ("keyword", "tenues sexy");
			ShowViewModel<FirstViewModel> (bundle);
		}

	}
}

