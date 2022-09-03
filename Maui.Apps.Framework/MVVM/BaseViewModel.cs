using CommunityToolkit.Mvvm.ComponentModel;

namespace Maui.Apps.Framework.MVVM;

public partial class BaseViewModel : ObservableObject
{
	[ObservableProperty]
	private string title = string.Empty;

	[ObservableProperty]
	private bool isBusy = false;

	[ObservableProperty]
	private string loadingText = string.Empty;

	[ObservableProperty]
	private bool dataLoaded = false;

	[ObservableProperty]
	private bool isErrorState = false;

	[ObservableProperty]
	private string errorMessage = string.Empty;

	[ObservableProperty]
	private string errorImage = string.Empty;

	public BaseViewModel()
		=> this.isErrorState = false;

	public virtual async void OnNavigatedTo(object parameters)
		=> await Task.CompletedTask;

	protected void SetDataLoadingIndicators(bool isStaring = true)
	{
		if (isStaring)
		{
			IsBusy = true;
			DataLoaded = false;
			IsErrorState = false;
			ErrorMessage = string.Empty;
			ErrorImage = string.Empty;
		}
		else
		{
			LoadingText = string.Empty;
			IsBusy = false;
		}
	}
}
