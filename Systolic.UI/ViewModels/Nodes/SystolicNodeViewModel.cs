using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;
using Systolic.Core.Abstractions;
using Systolic.UI.Models.Implementations;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public partial class SystolicNodeViewModel : ViewModelBase, IProcessingNode<double>
{
	[ObservableProperty] private AvaloniaDictionary<string, double> _inputRegisters = new();
	[ObservableProperty] private INode? _parent;

	[ObservableProperty] private IList<IPin> _pins;

	[ObservableProperty] private ObservableCollection<ObservableKeyValuePair<string, double>> _registers =
		new();

	public SystolicNodeViewModel() : this(["A", "B", "C"])
	{
	}

	public SystolicNodeViewModel(IEnumerable<string> registerNames)
	{
		foreach (var registerName in registerNames)
		{
			Registers.Add(new ObservableKeyValuePair<string, double>(registerName, default!));
			InputRegisters[registerName] = default!;
			Operations[registerName] = inputRegisters => inputRegisters[registerName];
		}
	}

	private Dictionary<string, Func<IDictionary<string, double>, double>> Operations { get; } =
		new();

	public void SetRegister(string registerName, double value)
	{
		InputRegisters[registerName] = value;
	}

	public void PerformOperations()
	{
		foreach (var register in Registers) register.Value = Operations[register.Key](InputRegisters);
	}

	public void ShiftRegisters()
	{
		foreach (var link in Pins.OfType<ExtendedPinViewModel>().Where(t => t.PinType == PinType.Output))
			link.SetRegister(link.Name!, Registers.First(t => t.Key == link.Name!).Value);
	}

	public void ResetRegisters()
	{
		foreach (var register in InputRegisters) InputRegisters[register.Key] = default!;
	}
}