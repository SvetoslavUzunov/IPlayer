using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.ViewModels.Base;
using IPlayer.Views;
using Maui.Apps.Framework.Exceptions;
using Maui.Apps.Framework.Extensions;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using static IPlayer.Models.Constants;

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

	[ObservableProperty]
	private double progressValue;

	[ObservableProperty]
	private bool isDownloading = false;

	private IEnumerable<MuxedStreamInfo> streamInfo;

	private readonly IDownloadFileService downloadFileService;

	public VideoDetailsPageViewModel(IYoutubeService apiService, IDownloadFileService downloadFileService) : base(apiService)
	{
		this.Title = ApplicationName;

		this.downloadFileService = downloadFileService;
	}

	public override async void OnNavigatedTo(object parameters)
	{
		var videoId = (string)parameters;

		SetDataLoadingIndicators(true);

		this.LoadingText = DefaultLoadingText;

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
			this.ErrorMessage = NoInternetConnectionMessage;
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

	//[RelayCommand]
	//private async Task UnlikeVideo()
	//	=> await PageService.DisplayAlert("Coming soon", "The unlike option is comming soon", "OK");

	[RelayCommand]
	private async Task ShareVideo()
	{
		var textToShare = $"{ShareVideoMessage}{TheVideo.Id}";

		await Share.RequestAsync(new ShareTextRequest
		{
			Text = textToShare,
			Title = TheVideo.Snippet.Title
		});
	}

	[RelayCommand]
	private async Task DownloadVideo()
	{
		if (IsDownloading)
			return;

		var progressIndicator = new Progress<double>((value) => ProgressValue = value);
		var cancellationTokenSource = new CancellationTokenSource();

		try
		{
			IsDownloading = true;

			var urlToDownload = streamInfo.
				OrderByDescending(video => video.VideoResolution.Area)
				.First().Url;

			var downloadedFilePath = await downloadFileService.
				DownloadFileAsync(urlToDownload, TheVideo.Snippet.Title.
					CleanCacheKey() + ".mp4", progressIndicator, cancellationTokenSource.Token);

			await Share.RequestAsync(new ShareFileRequest
			{
				Title = TheVideo.Snippet.Title,
				File = new ShareFile(downloadedFilePath)
			});
		}
		catch { }
		finally
		{
			IsDownloading = false;
		}
	}

	//[RelayCommand]
	//private async Task SubscribeChannel()
	//	=> await PageService.DisplayAlert("Coming Soon", "The subscribe to channel option is coming soon", "OK");

	[RelayCommand]
	private async Task NavigateToVideoDetailsPage(string videoId)
		=> await NavigationService.PushAsync(new VideosDetailsPage(videoId));

	private async Task GetVideoURL()
	{
		await Task.Run(async () =>
		{
			var youtube = new YoutubeClient();

			var stramManifest = await youtube.Videos.Streams.GetManifestAsync($"{YouTubeURL}{TheVideo.Id}");

			streamInfo = stramManifest.GetMuxedStreams();

			var videoPlayerStream = streamInfo.First(video => video.VideoQuality.Label is "240p" or "360p" or "480p");

			VideoSource = videoPlayerStream.Url;
		});
	}
}
