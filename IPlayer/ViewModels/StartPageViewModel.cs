using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.ViewModels.Base;
using Maui.Apps.Framework.Exceptions;
using Maui.Apps.Framework.Extensions;

namespace IPlayer.ViewModels;

public partial class StartPageViewModel : AppViewModelBase
{
	private string nextToken = string.Empty;
	private readonly string searchTerm = "iPhone 14";

	[ObservableProperty]
	private ObservableCollection<YoutubeVideo> youtubeVideos;

	public StartPageViewModel(IApiService apiService) : base(apiService)
	{
		this.Title = "IPlayer";
	}

	public override async void OnNavigatedTo(object parameters)
	{
		await SearchAsync();
	}

	private async Task SearchAsync()
	{
		SetDataLoadingIndicators(true);

		LoadingText = "Hold on, we are loading!";

		YoutubeVideos = new();

		try
		{
			await GetYouTubeVideoAsync();

			this.DataLoaded = true;
		}
		catch (InternetConnectionException)
		{
			this.IsErrorState = true;
			this.ErrorMessage = "Slow or no internet connection";
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

	private async Task GetYouTubeVideoAsync()
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
	private async void OpenSettingPage()
	{
		await PageService.DisplayAlert("Setting", "This implementations is outside the scope of this course.", "Got it.");
	}
}
