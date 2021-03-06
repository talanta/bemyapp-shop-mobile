﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using ModernHttpClient;
using Newtonsoft.Json;

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
		IList<string> _errors;
		ProductResult _lastResult;

		public DiscountService ()
		{
			_errors = new List<string> ();
		}

		#region IDiscountService implementation

		public ProductResult GetLastResult ()
		{
			var result = _lastResult;
			_lastResult = null;
			return result;
		}


		public string GetLastError ()
		{
			if (_errors.Count == 0)
				return null;
			return _errors [_errors.Count - 1];
		}

		public async Task<bool> AppendResultFromSearchProdudct (SearchRequest request)
		{
			var result = await SearchProdudct (request);

			if (result.ItemCount > 0) {
				_lastResult = result;
				return true;
			}
			return false;
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

