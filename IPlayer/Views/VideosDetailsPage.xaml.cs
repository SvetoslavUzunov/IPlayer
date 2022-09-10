using IPlayer.ViewModels;
using IPlayer.Views.Base;

namespace IPlayer.Views;

public partial class VideosDetailsPage : BaseView<VideoDetailsPageViewModel>
{
	public VideosDetailsPage(object initParams) : base(initParams)
	{
		InitializeComponent();
	}
}