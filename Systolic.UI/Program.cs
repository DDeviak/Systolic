using System;
using Avalonia;

namespace Systolic.UI;

internal class Program
{
	static Program()
	{
		App.EnableInputOutput = true;
		App.EnableMainMenu = true;
	}

	[STAThread]
	public static void Main(string[] args)
	{
		BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);
	}

	public static AppBuilder BuildAvaloniaApp()
	{
		return AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace()
			.UseSkia();
	}
}