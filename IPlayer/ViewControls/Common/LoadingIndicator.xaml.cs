namespace IPlayer.ViewControls.Common;

public partial class LoadingIndicator : VerticalStackLayout
{
	public LoadingIndicator()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
	   nameof(IsBusy),
	   typeof(bool),
	   typeof(LoadingIndicator),
	   false,
	   BindingMode.OneWay,
	   null,
	   SetIsBusy);

	public bool IsBusy
	{
		get => (bool)this.GetValue(IsBusyProperty);
		set => this.SetValue(IsBusyProperty, value);
	}

	private static void SetIsBusy(BindableObject bindable, object oldValue, object newValue)
	{
		LoadingIndicator control = bindable as LoadingIndicator;

		control.IsVisible = (bool)newValue;
		control.actIndicator.IsRunning = (bool)newValue;
	}

	public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
		nameof(LoadingText),
		typeof(string),
		typeof(LoadingIndicator),
		string.Empty,
		BindingMode.OneWay,
		null,
		SetLoadingText);

	public string LoadingText
	{
		get => (string)this.GetValue(LoadingTextProperty);
		set => this.SetValue(LoadingTextProperty, value);
	}

	private static void SetLoadingText(BindableObject bindable, object oldValue, object newValue)
		=> (bindable as LoadingIndicator).lableLoadingText.Text = (string)newValue;
}