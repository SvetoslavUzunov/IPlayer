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
	private readonly string searchTerm = string.Empty;

	[ObservableProperty]
	private ObservableCollection<YoutubeVideo> youtubeVideos;

	public StartPageViewModel(IApiService apiService) : base(apiService)
	{
		this.Title = "IPlayer";
	}

	public override void OnNavigatedTo(object parameters)
	{
		Search();
	}

	private async void Search()
	{
		SetDataLoadingIndicators(true);

		LoadingText = "Hold on, we are loading!";

		youtubeVideos = new();

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
		catch (Exception)
		{
			this.IsErrorState = true;
			this.ErrorMessage = "Something went wrong.";
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

		YoutubeVideos.AddRange(videoSearchResult.Items);
	}

	[RelayCommand]
	private async void OpenSettingPage()
	{
		await PageService.DisplayAlert("Setting", "This implementations is outside the scope of this course.", "Got it.");
	}
}
