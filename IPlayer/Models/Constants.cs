namespace IPlayer.Models;

public static class Constants
{
	public static string ApplicationName = "IPlayer";
	public static string EmailAddress = "iplayer@gmail.com";
	public static string ApplicationId = "SU.IPlayer.App";
	public static string ApiServiceURL = @"https://youtube.googleapis.com/youtube/v3/";
	public static string ApiKey = @"AIzaSyD10hggq6-GjzEz-jNt-myCRFXtzKOMDbs";

	public static uint MicroDuration { get; set; } = 100;
	
	public static uint SmallDuration { get; set; } = 300;
	
	public static uint MediumDuration { get; set; } = 600;
	
	public static uint LongDuration { get; set; } = 1200;
	
	public static uint ExtraLongDuration { get; set; } = 1800;
}
