using System;
using System.Threading.Tasks;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Cirrious.CrossCore;
using System.Collections.Generic;

namespace weshop.portable
{
	public class FullRequest 
	{
		public string ApiKey { get; set; }
		public SearchRequest SearchRequest { get; set; }
	}

	public class FullProductRequest 
	{
		public string ApiKey { get; set; }
		public ProductRequest ProductRequest { get; set; }
	}


	public class DiscountService : IDiscountService
	{
		IList<string> errors;

		public DiscountService ()
		{
			errors = new List<string> ();
		}

		#region IDiscountService implementation

		public string GetLastError ()
		{
			if (errors.Count == 0)
				return null;
			return errors [errors.Count - 1];
		}


		public async Task<ProductResult> GetProduct (ProductRequest request)
		{
			string rawResponse = null;
			Uri url = new Uri(Constants.URL_GET_PRODUCT);
			var fullreq = new FullProductRequest{ ApiKey = Constants.API_KEY, ProductRequest = request };
			string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(fullreq);
			var content = new StringContent(jsonData);

			using (var client = new HttpClient(new NativeMessageHandler()))
			{
				try
				{		
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
					//errors.Add (jsonData);
					Mvx.Trace (ex.Message);
				}
				finally{
					Mvx.Trace (rawResponse);
				}
				return null;
			}
		}

		public async Task<ProductResult> SearchProdudct (SearchRequest request)
		{
			//request.ApiKey = Constants.API_KEY;
			string rawResponse = null;
			string jsonData = null;
			Uri url = new Uri(Constants.URL_SEARCH);

			var fullreq = new FullRequest{ ApiKey = Constants.API_KEY, SearchRequest = request };
			using (var client = new HttpClient(new NativeMessageHandler()))
			{
				try
				{
					jsonData = Newtonsoft.Json.JsonConvert
						.SerializeObject(fullreq);
				var content = new StringContent(jsonData);
	

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
					//errors.Add (jsonData);
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

