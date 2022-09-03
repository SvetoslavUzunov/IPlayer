using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Maui.Apps.Framework.Exceptions;
using MonkeyCache;
using static Maui.Apps.Framework.Extensions.StringExtension;

namespace Maui.Apps.Framework.Services;

public class RestServiceBase
{
	private HttpClient httpClient;
	private readonly IBarrel cacheBarrel;
	private readonly IConnectivity connectivity;

	protected RestServiceBase(IBarrel cacheBarrel, IConnectivity connectivity)
	{
		this.cacheBarrel = cacheBarrel;
		this.connectivity = connectivity;
	}

	protected void SetBaseURL(string apiBaseUrl)
	{
		httpClient = new HttpClient
		{
			BaseAddress = new Uri(apiBaseUrl)
		};

		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	protected void AddHttpHeader(string key, string value)
		=> httpClient.DefaultRequestHeaders.Add(key, value);

	protected async Task<T> GetJsonAsync<T>(string resource, int cacheDuration = 24)
	{
		var json = await GetJsonBaseAsync(resource, cacheDuration);

		return JsonSerializer.Deserialize<T>(json);
	}

	private async Task<string> GetJsonBaseAsync(string resource, int cacheDuration = 24)
	{
		var cleanCacheKey = resource.CleanCacheKey();

		if (cacheBarrel is not null)
		{
			var cacheData = cacheBarrel.Get<string>(cleanCacheKey);

			if (cacheDuration > 0 && cacheData is not null && !cacheBarrel.IsExpired(cleanCacheKey))
			{
				return cacheData;
			}

			if (connectivity.NetworkAccess != NetworkAccess.Internet)
			{
				return cacheData is not null ? cacheData : throw new InternetConnectionException();
			}
		}

		if (connectivity.NetworkAccess != NetworkAccess.Internet)
		{
			throw new InternetConnectionException();
		}

		var response = await httpClient.GetAsync(new Uri(httpClient.BaseAddress, resource));
		response.EnsureSuccessStatusCode();

		string json = await response.Content.ReadAsStringAsync();

		if (cacheDuration > 0 && cacheBarrel is not null)
		{
			try
			{
				cacheBarrel.Add(cleanCacheKey, json, TimeSpan.FromHours(cacheDuration));
			}
			catch { }
		}

		return json;
	}

	protected async Task<HttpResponseMessage> PostJsonAsync<T>(string uri, T payload)
	{
		var data = JsonSerializer.Serialize(payload);
		var content = new StringContent(data, Encoding.UTF8, "application/json");

		var response = await httpClient.PostAsync(new Uri(httpClient.BaseAddress, uri), content);

		response.EnsureSuccessStatusCode();

		return response;
	}

	protected async Task<HttpResponseMessage> PutJsonAsync<T>(string uri, T payload)
	{
		var data = JsonSerializer.Serialize(payload);
		var content = new StringContent(data, Encoding.UTF8, "application/json");

		var response = await httpClient.PutAsync(new Uri(httpClient.BaseAddress, uri), content);

		response.EnsureSuccessStatusCode();

		return response;
	}

	protected async Task<HttpResponseMessage> DeleteJsonAsync<T>(string uri)
	{
		var response = await httpClient.DeleteAsync(new Uri(httpClient.BaseAddress, uri));

		response.EnsureSuccessStatusCode();

		return response;
	}
}
