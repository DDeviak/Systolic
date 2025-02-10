using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NodeEditor.Model;
using Systolic.UI.Models.Implementations;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public partial class InputSourceNodeViewModel : ViewModelBase
{
	[ObservableProperty] public string _currentInputIndex;

	[ObservableProperty] public string _currentInputName;
	[ObservableProperty] public double _currentInputValue;

	[ObservableProperty] public ObservableCollection<ObservableKeyValuePair<string, IList<double>>> _inputs =
		new();

	[ObservableProperty] public IList<IPin> _pins = new ObservableCollection<IPin>();

	[RelayCommand]
	public void AddInput(string inputName)
	{
		if (!Inputs.Where(i => i.Key == inputName).Any())
			Inputs.Add(new ObservableKeyValuePair<string, IList<double>>(inputName,
				new ObservableCollection<double>()));
	}

	[RelayCommand]
	public void ProvideInputs()
	{
		var pins = Pins.OfType<ExtendedPinViewModel>().Where(p => p.PinType == PinType.Output).Distinct()
			.ToDictionary(t => t.Name!, t => t);
		foreach (var input in Inputs)
			if (pins.TryGetValue(input.Key, out var pin))
			{
				pin.SetRegister(input.Key, input.Value.FirstOrDefault());
				if (input.Value.Count > 0) input.Value.RemoveAt(0);
			}
	}
}