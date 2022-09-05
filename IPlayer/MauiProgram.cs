using Android.App;
using Android.Views;
using Microsoft.Maui.LifecycleEvents;

namespace IPlayer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FiraSans-Light.ttf", "RegularFont");
				fonts.AddFont("FiraSans-Medium.ttf", "MediumFont");
			})
			.ConfigureLifecycleEvents(events =>
			{
#if ANDROID
				events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

				static void MakeStatusBarTranslucent(Activity activity)
				{
					activity.Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
					activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
					activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
				}
#endif
			});

		return builder.Build();
	}
}