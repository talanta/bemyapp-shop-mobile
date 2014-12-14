using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;

namespace weshop.portable
{
	public class ProductViewModel
	{
		public Product Product { get; set;}


		IMvxCommand showCmd;
		public IMvxCommand ShowCmd{ get {return showCmd ??(showCmd = new MvxCommand(OnShowCmd)); } }

		protected void OnShowCmd()
		{
			Mvx.Resolve<IDialogService> ()
				.ShowProduct (Product.BestOffer.ProductURL);
		}
	}

//	Electromenager
//	Maison déco
//	Bricolage
//	Jardin Animaler
//	Informatique
//	TV MP3
//	Telephonie
//	Photo et Camescope
//	Auto Moto et GPS
//	Vetement Chaussures
//	Bagages et  Bijouterie
//	Bebe Puericulture
//	Jeux Jouets
//	Jeux Video Culture
//	Sport
//	Vin et Supermarche
//
//	Mes Gros Outils
//	Mon Territoire
//	Ma caisse a outils
//	Mon jardin secret
//	Mes Gadgets
//	Mon Spectacle
//	Ma Douce Voix
//	Mes Images
//	Ma Direction
//	Ma GardeRobe
//	Mes Précieuses
//	Pour Mes Petits
//	Mes Toys
//	Mon Geek
//	Mon Corps
//	Ma Ligne
}


