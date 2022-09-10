using IPlayer.Models;

namespace IPlayer.IServices;

public interface IApiService
{
	public Task<VideoSearchResult> SearchVideosAsync(string searchQuery, string nextPageToken = "");

	public Task<ChannelSearchResult> GetChannelsAsync(string channelIDs);

	public Task<YoutubeVideoDetail> GetVideoDetailsAsync(string videoID);

	public Task<CommentsSearchResult> GetCommentsAsync(string videoID);
}
