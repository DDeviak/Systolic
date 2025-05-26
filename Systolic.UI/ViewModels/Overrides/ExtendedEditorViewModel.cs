using System.ComponentModel;
using NodeEditor.Mvvm;

namespace Systolic.UI.ViewModels.Overrides;

public class ExtendedEditorViewModel : EditorViewModel
{
	public ExtendedDrawingNodeViewModel? ExtendedDrawing => Drawing as ExtendedDrawingNodeViewModel;

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);
		if (e.PropertyName == nameof(Drawing))
		{
			base.OnPropertyChanged(new PropertyChangedEventArgs(nameof(ExtendedDrawing)));
		}
	}

	protected override void OnPropertyChanging(PropertyChangingEventArgs e)
	{
		base.OnPropertyChanging(e);
		if (e.PropertyName == nameof(Drawing))
		{
			base.OnPropertyChanging(new PropertyChangingEventArgs(nameof(ExtendedDrawing)));
		}
	}
}