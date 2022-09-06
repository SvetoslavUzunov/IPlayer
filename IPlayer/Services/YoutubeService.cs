using System.Net;
using IPlayer.IServices;
using IPlayer.Models;
using Maui.Apps.Framework.Services;
using MonkeyCache;

namespace IPlayer.Services;

public class YoutubeService : RestServiceBase, IApiService
{
	public YoutubeService(IBarrel barrel, IConnectivity connectivity) : base(barrel, connectivity)
	{
		SetBaseURL(Constants.ApiServiceURL);
	}

	public async Task<VideoSearchResult> SearchVideosAsync(string searchQuery, string nextPageToken = "")
	{
		var resourceUri = $"search?part=snippet&maxResults=10&type=video&key={Constants.ApiKey}&q={WebUtility.UrlEncode(searchQuery)}"
			+
			(!string.IsNullOrEmpty(nextPageToken) ? $"&pageToken={nextPageToken}" : "");

		return await GetJsonAsync<VideoSearchResult>(resourceUri, cacheDuration: 4);
	}
}
