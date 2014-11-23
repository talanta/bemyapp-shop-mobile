using System;
using System.Threading.Tasks;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Cirrious.CrossCore;

namespace weshop.portable
{


	public class DiscountService : IDiscountService
	{


		#region IDiscountService implementation

		public async Task<ProductResult> SearchProdudct (SearchRequest request)
		{
			//request.ApiKey = Constants.API_KEY;
			string rawResponse = null;
			Uri url = new Uri(Constants.URL_SEARCH);
			using (var client = new HttpClient(new NativeMessageHandler()))
			{
				try
				{
				string jsonData = Newtonsoft.Json.JsonConvert
						.SerializeObject(new {ApiKey = Constants.API_KEY, SearchRequest = request});
				var content = new StringContent(jsonData);
					Mvx.Trace (jsonData);

				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var result = await client.PostAsync(url, content);
					rawResponse = await result.Content.ReadAsStringAsync();
				if (result.IsSuccessStatusCode)
				{
					
					return JsonConvert.DeserializeObject<ProductResult>(rawResponse);

				}
				// to be handled...
				}
				catch(Exception ex) {
					Mvx.Trace (ex.Message);
				}
				finally{
					Mvx.Trace (rawResponse);
				}
				return null;
			}
		}

		#endregion


	}
}

