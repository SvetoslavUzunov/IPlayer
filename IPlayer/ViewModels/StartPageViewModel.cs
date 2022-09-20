using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.ViewModels.Base;
using IPlayer.Views;
using Maui.Apps.Framework.Exceptions;
using Maui.Apps.Framework.Extensions;
using static IPlayer.Models.Constants;

namespace IPlayer.ViewModels;

public partial class StartPageViewModel : AppViewModelBase
{
	private string nextToken = string.Empty;
	private string searchTerm = DefaultSearchTerm;

	[ObservableProperty]
	private ObservableCollection<YoutubeVideo> youtubeVideos;

	[ObservableProperty]
	private bool isLoadingMore;

	public StartPageViewModel(IYoutubeService apiService) : base(apiService)
		=> this.Title = ApplicationName;

	public override async void OnNavigatedTo(object parameters)
		=> await SearchAsync();

	private async Task SearchAsync()
	{
		SetDataLoadingIndicators(true);

		LoadingText = DefaultLoadingText;

		YoutubeVideos = new();

		try
		{
			await GetYouTubeVideosAsync();

			this.DataLoaded = true;
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

	private async Task GetYouTubeVideosAsync()
	{
		var videoSearchResult = await ApiService.SearchVideosAsync(searchTerm, nextToken);

		nextToken = videoSearchResult.NextPageToken;

		var channelIDs = string.Join(",", videoSearchResult.Items
				.Select(video => video.Snippet.ChannelId).Distinct());

		var channelSearchResult = await ApiService.GetChannelsAsync(channelIDs);

		videoSearchResult.Items
			.ForEach(video => video.Snippet.ChannelImageURL = channelSearchResult.Items
				.Where(channel => channel.Id == video.Snippet.ChannelId)
					.First().Snippet.Thumbnails.High.Url);

		YoutubeVideos.AddRange(videoSearchResult.Items);
	}

	[RelayCommand]
	private async Task LoadMoreVideos()
	{
		if (IsLoadingMore || string.IsNullOrEmpty(nextToken))
		{
			return;
		}

		IsLoadingMore = true;
		await Task.Delay(1000);
		await GetYouTubeVideosAsync();
		IsLoadingMore = false;
	}

	[RelayCommand]
	private async Task SearchVideos(string searchQuery)
	{
		if (searchQuery is not null && searchQuery != string.Empty)
		{
			nextToken = string.Empty;
			searchTerm = searchQuery.Trim();

			await SearchAsync();
		}
	}

	[RelayCommand]
	private async Task NavigateToVideoDetailsPage(string videoId)
		=> await NavigationService.PushAsync(new VideosDetailsPage(videoId));
}
