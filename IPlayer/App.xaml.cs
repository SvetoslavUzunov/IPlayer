#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

using IPlayer.Views;
using Microsoft.Maui.Handlers;

namespace IPlayer;

public partial class App : Application
{
	private const int WindowWidth = 540;
	private const int WindowHeight = 1000;

	public App()
	{
		InitializeComponent();

		VersionTracking.Track();

		WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
		{
#if WINDOWS
			var mauiWindow=handler.VirtualView;
			var nativeWindow=handler.PlatformView;
			nativeWindow.Activate();
			IntPtr windowHandle=WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
			WindowId windowId=Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
			AppWindow appWindow=Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
			appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
#endif
		});

		MainPage = new NavigationPage(new StartPage());
	}
}
