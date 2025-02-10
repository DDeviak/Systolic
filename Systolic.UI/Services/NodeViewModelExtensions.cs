using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.Services;

public static class NodeViewModelExtensions
{
	public static IPin AddPin(this NodeViewModel node, double x, double y, double width, double height,
		PinAlignment alignment = PinAlignment.None, string? name = null, PinType pinType = PinType.None)
	{
		var pin = new ExtendedPinViewModel
		{
			Name = name,
			Parent = node,
			X = x,
			Y = y,
			Width = width,
			Height = height,
			Alignment = alignment,
			PinType = pinType
		};

		node.Pins ??= new ObservableCollection<IPin>();
		node.Pins.Add(pin);

		return pin;
	}
}