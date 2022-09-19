using IPlayer.Models;
using IPlayer.ViewModels;
using IPlayer.Views.Base;

namespace IPlayer.Views;

public partial class VideosDetailsPage : BaseView<VideoDetailsPageViewModel>
{
	public VideosDetailsPage(object initParams) : base(initParams)
	{
		InitializeComponent();

		this.ViewModelInitialized += (s, e) =>
		{
			(this.BindingContext as VideoDetailsPageViewModel).DownloadCompleted += VideoDetailsPage_DownloadCompleted;
		};
	}

	protected override void OnDisappearing()
	{
		(this.BindingContext as VideoDetailsPageViewModel).DownloadCompleted -= VideoDetailsPage_DownloadCompleted;

		try
		{
			VideoPlayer.Stop();
		}
		catch { }

		base.OnDisappearing();
	}

	private void VideoDetailsPage_DownloadCompleted(object sender, EventArgs e)
	{
		if ((this.BindingContext as VideoDetailsPageViewModel).IsErrorState)
			return;

		if (this.AnimationIsRunning("TransitionAnimation"))
			return;

		var parentAnimation = new Animation()
		{
			{ 0.0, 0.7, new Animation(v => HeaderView.Opacity = v, 0, 1, Easing.CubicIn) }, // Poster Image Animation
			{ 0.4, 0.7, new Animation(v => VideoTitle.Opacity = v, 0, 1, Easing.CubicIn) }, // Video Title Animation
			{ 0.5, 0.7, new Animation(v => VideoIcons.Opacity = v, 0, 1, Easing.CubicIn) }, // Video Icons Animation
			{ 0.6, 0.8, new Animation(v => ChannelDetails.Opacity = v, 0, 1, Easing.CubicIn) }, // Channel Details Animation
			{ 0.7, 0.9, new Animation(v => SimilarVideos.Opacity = v, 0, 1, Easing.CubicIn) }, // Similar Videos Animation
			//{ 0.65, 0.85, new Animation(v => TagsView.Opacity = v, 0, 1, Easing.CubicIn) }, // Tags Animation
			{ 0.8, 1, new Animation(v => DescriptionView.Opacity = v, 0, 1, Easing.CubicIn) }, // Description View Animation
			{ 0.8, 1, new Animation(v => CommentsButton.Opacity = v, 0, 1, Easing.CubicIn) } // Comments Button Animation
		};

		parentAnimation.Commit(this, "TransitionAnimation", rate: 16, Constants.ExtraLongDuration, null,
			(v, c) =>
			{
				// Action to perform on completion
			});
	}

	private async void BtnComments_Clicked(object sender, EventArgs e)
		=> await CommentsBottomSheet.OpenBottomSheet();

	private void VideoPlayerButton_Clicked(object sender, EventArgs e)
	{
		VideoPlayer.IsVisible = true;
		VideoPlayer.Play();
	}
}