using CommunityToolkit.Mvvm.Input;
using IPlayer.IServices;
using Maui.Apps.Framework.MVVM;

namespace IPlayer.ViewModels.Base;

public partial class AppViewModelBase : BaseViewModel
{
	public AppViewModelBase(IApiService apiService) : base()
		=> this.apiService = apiService;

	public INavigation NavigationService { get; set; }

	public Page PageService { get; set; }

	protected IApiService apiService { get; set; }

	[RelayCommand]
	private async Task NavigationBack()
		=> await NavigationService.PopAsync();

	[RelayCommand]
	private async Task CloseModal()
		=> await NavigationService.PopModalAsync();
}
