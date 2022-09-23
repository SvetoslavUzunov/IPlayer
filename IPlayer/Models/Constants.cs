namespace IPlayer.Models;

public static class Constants
{
	public const string ApplicationName = "VideoPlay";
	public const string EmailAddress = "videoPlay@gmail.com";
	public const string ApplicationId = "SU.IPlayer.App";
	public const string ApiServiceURL = @"https://youtube.googleapis.com/youtube/v3/";
	public const string YouTubeURL = "https://www.youtube.com/watch?v=";
	public const string ApiKey = @"PASTE HERE YOUR OWN API KEY";

	public const string DefaultSearchTerm = "YouTube";
	public const string DefaultLoadingText = "Hold on, we are loading!";
	public const string DownloadVideoExtension = ".mp4";
	public const string ForbiddenExceptionCode = "403";
	public const int LoadingNextVideosTime = 1000;

	public const string ShareVideoMessage = $"Hey, I found this amazing video. Check it out: {YouTubeURL}";

	public const string NoInternetConnectionMessage = "Slow or no internet connection!";
	public const string PrivateVideoMessage = "This video is private!";

	public static uint MicroDuration { get; set; } = 100;

	public static uint SmallDuration { get; set; } = 300;

	public static uint MediumDuration { get; set; } = 600;

	public static uint LongDuration { get; set; } = 1200;

	public static uint ExtraLongDuration { get; set; } = 1800;
}
