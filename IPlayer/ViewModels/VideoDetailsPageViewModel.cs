using CommunityToolkit.Mvvm.ComponentModel;
using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.ViewModels.Base;
using Maui.Apps.Framework.Exceptions;

namespace IPlayer.ViewModels;

public partial class VideoDetailsPageViewModel : AppViewModelBase
{
	[ObservableProperty]
	private YoutubeVideoDetail theVideo;

	[ObservableProperty]
	private List<YoutubeVideo> similarVideos;

	[ObservableProperty]
	private Channel theChannel;

	[ObservableProperty]
	private bool similarVideosAvailable;

	[ObservableProperty]
	private List<Comment> comments;

	public event EventHandler DownloadCompleted;

	public VideoDetailsPageViewModel(IApiService apiService) : base(apiService)
	{
		this.Title = "IPlayer";
	}

	public override async void OnNavigatedTo(object parameters)
	{
		var videoId = (string)parameters;

		SetDataLoadingIndicators(true);

		this.LoadingText = "Hold on...";

		try
		{
			SimilarVideos = new();
			Comments = new();

			TheVideo = await ApiService.GetVideoDetailsAsync(videoId);

			var channelSearchResult = await ApiService.GetChannelsAsync(TheVideo.Snippet.ChannelId);
			TheChannel = channelSearchResult.Items.First();

			if (TheVideo.Snippet.Tags is not null)
			{
				var similarVideosSearchResult = await ApiService.SearchVideosAsync(TheVideo.Snippet.Tags.First(), "");

				SimilarVideos = similarVideosSearchResult.Items;
				SimilarVideosAvailable = (SimilarVideos?.Count > 0);
			}

			var commentsSearchResult = await ApiService.GetCommentsAsync(videoId);
			Comments = commentsSearchResult.Items;

			this.DataLoaded = true;

			DownloadCompleted?.Invoke(this, new EventArgs());
		}
		catch (InternetConnectionException)
		{
			this.IsErrorState = true;
			this.ErrorMessage = "Slow or no internet connection.";
			this.ErrorImage = "nointernet.png";
		}
		catch (Exception ex)
		{
			this.IsErrorState = true;
			this.ErrorMessage = ex.Message;
			this.ErrorImage = "error.png";
		}
		finally
		{
			SetDataLoadingIndicators(false);
		}
	}
}
