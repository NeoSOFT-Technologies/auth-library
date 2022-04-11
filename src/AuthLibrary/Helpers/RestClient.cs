using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthLibrary.Helpers
{
	public class RestClient : IDisposable
	{
		private HttpClient httpClient;
		protected readonly string _baseAddress;
		private readonly string _addressSuffix;
		private bool disposed = false;

		public RestClient(string baseAddress, string addressSuffix)
		{
			_baseAddress = baseAddress;
			_addressSuffix = addressSuffix;
			httpClient = CreateHttpClient(_baseAddress);
		}

		protected virtual HttpClient CreateHttpClient(string serviceBaseAddress)
		{
			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(serviceBaseAddress);
			return httpClient;
		}

		public async Task<HttpResponseMessage> PostAsync(KeyValuePair<string, string>[] requestBody, Dictionary<string, string> headers = null)
		{
			if (headers != null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
			}
			HttpResponseMessage responseMessage = await httpClient.PostAsync(_addressSuffix, new FormUrlEncodedContent(requestBody));
			return responseMessage;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				if (httpClient != null)
				{
					httpClient.Dispose();
				}
				disposed = true;
			}
		}
	}
}
