using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.ViewModels.Base;
using IPlayer.Views;
using Maui.Apps.Framework.Exceptions;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

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
	private List<Comment> comments;

	public event EventHandler DownloadCompleted;

	[ObservableProperty]
	private string videoSource;

	private IEnumerable<MuxedStreamInfo> streamInfo;

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
			}

			var commentsSearchResult = await ApiService.GetCommentsAsync(videoId);
			Comments = commentsSearchResult.Items;

			await GetVideoURL();

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

	[RelayCommand]
	private async Task UnlikeVideo()
		=> await PageService.DisplayAlert("Coming soon", "The unlike option is comming soon", "OK");

	[RelayCommand]
	private async Task ShareVideo()
	{
		var textToShare = $"Hey, I found this amazing video. Check it out: https://www.youtube.com/watch?v{TheVideo.Id}";

		await Share.RequestAsync(new ShareTextRequest
		{
			Text = textToShare,
			Title = TheVideo.Snippet.Title
		});
	}

	[RelayCommand]
	private void DownloadVideo()
	{

	}

	[RelayCommand]
	private async Task SubscribeChannel()
		=> await PageService.DisplayAlert("Coming Soon", "The subscribe to channel option is coming soon", "OK");

	[RelayCommand]
	private async Task NavigateToVideoDetailsPage(string videoId)
		=> await NavigationService.PushAsync(new VideosDetailsPage(videoId));

	private async Task GetVideoURL()
	{
		var youtube = new YoutubeClient();

		var stramManifest = await youtube.Videos.Streams.GetManifestAsync($"https://youtube.com/watch?v={TheVideo.Id}");

		streamInfo = stramManifest.GetMuxedStreams();

		var videoPlayerStream = streamInfo.First(video => video.VideoQuality.Label is "240p" or "360p" or "480p");

		VideoSource = videoPlayerStream.Url;
	}
}
