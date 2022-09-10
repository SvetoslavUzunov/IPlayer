namespace IPlayer.ViewControls;

public partial class VideoCell : ContentView
{
	public VideoCell()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty ParentContextProperty = BindableProperty.Create(
		"ParentContext",
		typeof(object),
		typeof(VideoCell),
		null,
		propertyChanged:
		(bindableObject, oldValue, newValue) =>
		{
			if (newValue is not null && bindableObject is VideoCell cell && newValue != oldValue)
			{
				cell.ParentContext = newValue;
			}
		});

	public object ParentContext
	{
		get => GetValue(ParentContextProperty);
		set => SetValue(ParentContextProperty, value);
	}
}