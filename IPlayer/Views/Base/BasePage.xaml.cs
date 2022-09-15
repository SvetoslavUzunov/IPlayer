using CommunityToolkit.Mvvm.Input;
using Maui.Apps.Framework.UI;

namespace IPlayer.Views.Base;

public partial class BasePage : ContentPage
{
	public BasePage()
	{
		InitializeComponent();

		NavigationPage.SetHasNavigationBar(this, false);

		SetPageMode(PageMode.None);

		SetContentDisplayMode(ContentDisplayMode.NoNavigationBar);
	}

	public IList<IView> PageContent => PageContentGrid.Children;

	public IList<IView> PageIcons => PageIconsGrid.Children;

	protected bool IsBackButtonEnabled
	{
		set => NavigateBackButton.IsEnabled = value;
	}

	public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(
		nameof(PageTitle),
		typeof(string),
		typeof(BasePage),
		string.Empty,
		defaultBindingMode:
		BindingMode.OneWay,
		propertyChanged: OnPageTitleChanged);

	public string PageTitle
	{
		get => (string)GetValue(PageTitleProperty);
		set => SetValue(PageTitleProperty, value);
	}

	private static void OnPageTitleChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is BasePage basePage)
		{
			basePage.TitleLabel.Text = (string)newValue;
			basePage.TitleLabel.IsVisible = true;
		}
	}

	public static readonly BindableProperty PageModeProperty = BindableProperty.Create(
		nameof(PageMode),
		typeof(PageMode),
		typeof(BasePage),
		PageMode.None,
		propertyChanged: OnPageModePropertyChanged);

	public PageMode PageMode
	{
		get => (PageMode)GetValue(PageModeProperty);
		set => SetValue(PageModeProperty, value);
	}

	private static void OnPageModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is BasePage basePage)
		{
			basePage.SetPageMode((PageMode)newValue);
		}
	}

	private void SetPageMode(PageMode pageMode)
	{
		switch (pageMode)
		{
			case PageMode.Menu:
				NavigateBackButton.IsVisible = false;
				CloseButton.IsVisible = false;
				break;
			case PageMode.Navigate:
				NavigateBackButton.IsVisible = true;
				CloseButton.IsVisible = false;
				break;
			case PageMode.Modal:
				NavigateBackButton.IsVisible = false;
				CloseButton.IsVisible = true;
				break;
			default:
				NavigateBackButton.IsVisible = false;
				CloseButton.IsVisible = false;
				break;
		}
	}

	public static readonly BindableProperty ContentDisplayModeProperty = BindableProperty.Create(
		nameof(ContentDisplayMode),
		typeof(ContentDisplayMode),
		typeof(BasePage),
		ContentDisplayMode.NoNavigationBar,
		propertyChanged: OnContentDisplayModePropertyChanged);

	public ContentDisplayMode ContentDisplayMode
	{
		get => (ContentDisplayMode)GetValue(ContentDisplayModeProperty);
		set => SetValue(ContentDisplayModeProperty, value);
	}

	private static void OnContentDisplayModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable != null && bindable is BasePage basePage)
		{
			basePage.SetContentDisplayMode((ContentDisplayMode)newValue);
		}
	}

	private void SetContentDisplayMode(ContentDisplayMode contentDisplayMode)
	{
		switch (contentDisplayMode)
		{
			case ContentDisplayMode.NavigationBar:
				Grid.SetRow(PageContentGrid, 2);
				Grid.SetRowSpan(PageContentGrid, 1);
				break;
			case ContentDisplayMode.NoNavigationBar:
				Grid.SetRow(PageContentGrid, 0);
				Grid.SetRowSpan(PageContentGrid, 3);
				break;
			default:
				break;
		}
	}
}