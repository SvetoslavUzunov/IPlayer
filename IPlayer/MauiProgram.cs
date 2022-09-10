﻿using IPlayer.IServices;
using IPlayer.Models;
using IPlayer.Services;
using IPlayer.ViewModels;
using Microsoft.Maui.LifecycleEvents;
using MonkeyCache.FileStore;

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

				static void MakeStatusBarTranslucent(Android.App.Activity activity)
				{
					activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
					activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
					activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
				}
#endif
			});

		RegisterAppServices(builder.Services);

		return builder.Build();
	}

	private static void RegisterAppServices(IServiceCollection services)
	{
		services.AddSingleton(Connectivity.Current);

		Barrel.ApplicationId = Constants.ApplicationId;
		services.AddSingleton(Barrel.Current);

		services.AddSingleton<IApiService, YoutubeService>();
		services.AddSingleton<StartPageViewModel>();
		services.AddTransient<VideoDetailsPageViewModel>();
	}
}