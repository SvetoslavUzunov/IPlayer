using IPlayer.Models;

namespace IPlayer.IServices;

public interface IApiService
{
	public Task<VideoSearchResult> SearchVideos(string searchQuery, string nectPageToken = "");
}
