using System;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace weshop.portable
{
	public class NetworkMessage: MvxMessage
	{
		public NetworkMessage (object sender) 
			: base(sender)
		{
			
		}
	}
}

