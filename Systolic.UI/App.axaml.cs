using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Systolic.UI.Services;
using Systolic.UI.ViewModels;
using Systolic.UI.ViewModels.Overrides;
using Systolic.UI.Views;

namespace Systolic.UI;

public class App : Application
{
	public static bool EnableInputOutput { get; set; } = true;

	public static bool EnableMainMenu { get; set; } = true;

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		var vm = new MainViewViewModel
		{
			IsToolboxVisible = true
		};

		var editor = new ExtendedEditorViewModel()
		{
			Serializer = new NodeSerializer(typeof(ObservableCollection<>)),
			Factory = new NodeFactory()
		};

		editor.Templates = editor.Factory.CreateTemplates();
		editor.Drawing = editor.Factory.CreateDrawing();
		editor.Drawing.SetSerializer(editor.Serializer);

		vm.Editor = editor;

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow
			{
				DataContext = vm
			};

			DataContext = vm;
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
		{
			singleViewLifetime.MainView = new MainView
			{
				DataContext = vm
			};

			DataContext = vm;
		}

		base.OnFrameworkInitializationCompleted();
	}
}