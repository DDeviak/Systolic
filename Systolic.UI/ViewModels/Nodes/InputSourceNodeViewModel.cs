using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.UI.Services;
using Systolic.UI.ViewModels.Abstractions;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public partial class InputSourceNodeViewModel : ViewModelBase, IInputProvider
{
	[ObservableProperty]
	private ObservableCollection<Models.ObservableKeyValuePair<string, ObservableCollection<double>>> _inputs =
		new();

	[ObservableProperty] private INode _parent = null!;

	[RelayCommand]
	public void ProvideInputs()
	{
		var pins = Parent.Pins.OfType<ExtendedPinViewModel>().Where(p => p.PinType == PinType.Output).Distinct()
			.ToDictionary(t => t.Name!, t => t);
		foreach (var input in Inputs)
			if (pins.TryGetValue(input.Key, out var pin))
			{
				pin.SetRegister(input.Key, input.Value.FirstOrDefault());
				if (input.Value.Count > 0) input.Value.RemoveAt(0);
			}

		for (var i = 0; i < Inputs.Count; i++) Inputs[i] = Inputs[i];
	}

	[RelayCommand]
	public void AddInput(string inputName)
	{
		if (Inputs.All(i => i.Key != inputName))
		{
			var collection = new ObservableCollection<double>();
			var pair = new Models.ObservableKeyValuePair<string, ObservableCollection<double>>(inputName, collection);
			Inputs.Add(pair);

			(Parent as NodeViewModel)!.AddPin(0, 0, 10, 10, PinAlignment.Left, inputName,
				PinType.Output);
		}
	}
	
	[RelayCommand]
	public void RemoveInput(string inputName)
	{
		if (Inputs.All(i => i.Key != inputName)) return;
		var pair = Inputs.First(i => i.Key == inputName);
		Inputs.Remove(pair);
		var pin = (Parent as NodeViewModel)!.Pins!.OfType<ExtendedPinViewModel>()
			.First(t => t.Name == inputName);
		(Parent as NodeViewModel)!.Pins!.Remove(pin);
	}
}