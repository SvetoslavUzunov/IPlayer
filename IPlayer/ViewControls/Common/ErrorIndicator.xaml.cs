namespace IPlayer.ViewControls.Common;

public partial class ErrorIndicator : VerticalStackLayout
{
	public ErrorIndicator()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty IsErrorProperty = BindableProperty.Create(
		nameof(IsError),
		typeof(bool),
		typeof(ErrorIndicator),
		false,
		BindingMode.OneWay,
		null,
		SetIsError);

	public bool IsError
	{
		get => (bool)this.GetValue(IsErrorProperty);
		set => this.SetValue(IsErrorProperty, value);
	}

	private static void SetIsError(BindableObject bindable, object oldValue, object newValue)
		=> (bindable as ErrorIndicator).IsVisible = (bool)newValue;


	public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
		nameof(ErrorText),
		typeof(string),
		typeof(ErrorIndicator),
		string.Empty,
		BindingMode.OneWay,
		null,
		SetErrorText);

	public string ErrorText
	{
		get => (string)this.GetValue(ErrorTextProperty);
		set => this.SetValue(ErrorTextProperty, value);
	}

	private static void SetErrorText(BindableObject bindable, object oldValue, object newValue)
		=> (bindable as ErrorIndicator).labelErrorText.Text = (string)newValue;


	public static readonly BindableProperty ErrorImageProperty = BindableProperty.Create(
		nameof(ErrorImage),
		typeof(ImageSource),
		typeof(ErrorIndicator),
		null,
		BindingMode.OneWay,
		null,
		SetErrorImage);

	public ImageSource ErrorImage
	{
		get => (ImageSource)this.GetValue(ErrorImageProperty);
		set => this.SetValue(ErrorImageProperty, value);
	}

	private static void SetErrorImage(BindableObject bindable, object oldValue, object newValue)
		=> (bindable as ErrorIndicator).imgError.Source = (ImageSource)newValue;
}