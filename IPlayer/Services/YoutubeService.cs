using System.Net;
using IPlayer.IServices;
using IPlayer.Models;
using Maui.Apps.Framework.Services;
using MonkeyCache;

namespace IPlayer.Services;

public class YoutubeService : RestServiceBase, IYoutubeService
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

	public async Task<ChannelSearchResult> GetChannelsAsync(string channelIDs)
	{
		var resourceUri = $"channels?part=snippet,statistics&maxResults=10&key={Constants.ApiKey}&id={channelIDs}";

		return await GetJsonAsync<ChannelSearchResult>(resourceUri, cacheDuration: 4);
	}

	public async Task<YoutubeVideoDetail> GetVideoDetailsAsync(string videoId)
	{
		var resourceUri = $"videos?part=contentDetails,id,snippet,statistics&key={Constants.ApiKey}&id={videoId}";

		var result = await GetJsonAsync<VideoDetailsResult>(resourceUri, cacheDuration: 24);

		return result.Items.First();
	}

	public Task<CommentsSearchResult> GetCommentsAsync(string videoId)
	{
		var resourceUri = $"commentThreads?part=snippet&maxResults=100&key={Constants.ApiKey}&videoId={videoId}";

		return GetJsonAsync<CommentsSearchResult>(resourceUri, cacheDuration: 4);
	}
}
