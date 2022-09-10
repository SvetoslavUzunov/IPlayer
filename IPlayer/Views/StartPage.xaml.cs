using IPlayer.ViewModels;
using IPlayer.Views.Base;

namespace IPlayer.Views;

public partial class StartPage : BaseView<StartPageViewModel>
{
	public StartPage()
	{
		InitializeComponent();
	}

	public static BindableProperty ItemsHeightProperty = BindableProperty.Create(
		nameof(ItemsHeight),
		typeof(double),
		typeof(StartPage),
		null,
		BindingMode.OneWay);

	public double ItemsHeight
	{
		get => (double)GetValue(ItemsHeightProperty);
		set => SetValue(ItemsHeightProperty, value);
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		ItemsHeight = 60d + (width - listVideos.Margin.Right - listVideos.Margin.Left) / 1.8d;
	}

	private void TextSearchQuery_Completed(object sender, EventArgs e)
		=> ViewModel.SearchVideosCommand.Execute(TextSearchQuery.Text);
}