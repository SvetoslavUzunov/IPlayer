using IPlayer.Helpers;
using IPlayer.ViewModels.Base;

namespace IPlayer.Views.Base;

public class BaseView<TViewModel> : BasePage where TViewModel : AppViewModelBase
{
	protected bool isLoaded = false;

	public BaseView() : base() { }

	public BaseView(object initParameters) : base()
		=> ViewModelParameters = initParameters;

	protected TViewModel ViewModel { get; set; }

	protected object ViewModelParameters { get; set; }

	protected event EventHandler ViewModelInitialized;

	protected override void OnAppearing()
	{
		if (!isLoaded)
		{
			base.OnAppearing();

			BindingContext = ViewModel = ServiceHelpers.GetService<TViewModel>();

			ViewModel.NavigationService = this.Navigation;
			ViewModel.PageService = this;

			ViewModelInitialized?.Invoke(this, new EventArgs());
			ViewModel.OnNavigatedTo(ViewModelParameters);

			isLoaded = true;
		}
	}
}
