using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using Maui.Apps.Framework.MVVM;

namespace IPlayer.ViewModels.Base;

public partial class AppViewModelBase : BaseViewModel
{
	public AppViewModelBase(IYoutubeService apiService) : base()
		=> this.ApiService = apiService;

	public INavigation NavigationService { get; set; }

	public Page PageService { get; set; }

	protected IYoutubeService ApiService { get; set; }

	[RelayCommand]
	private async Task NavigateBack()
		=> await NavigationService.PopAsync();

	[RelayCommand]
	private async Task CloseModal()
		=> await NavigationService.PopModalAsync();
}
