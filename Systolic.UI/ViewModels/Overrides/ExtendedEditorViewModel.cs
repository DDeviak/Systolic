using NodeEditor.Mvvm;

namespace Systolic.UI.ViewModels.Overrides;

public class ExtendedEditorViewModel : EditorViewModel
{
	public ExtendedDrawingNodeViewModel? ExtendedDrawing => Drawing as ExtendedDrawingNodeViewModel;
}