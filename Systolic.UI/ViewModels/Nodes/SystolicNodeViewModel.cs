using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.Core.Abstractions;
using Systolic.UI.Models.Implementations;
using Systolic.UI.Services;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public partial class SystolicNodeViewModel : ViewModelBase, IProcessingNode<double>
{
	[ObservableProperty] private AvaloniaDictionary<string, double> _inputRegisters = new();
	[ObservableProperty] private INode? _parent;

	[ObservableProperty] private ObservableCollection<ObservableKeyValuePair<string, double>> _registers =
		new();

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
		foreach (var link in Parent.Pins.OfType<ExtendedPinViewModel>().Where(t => t.PinType == PinType.Output))
			link.SetRegister(link.Name!, Registers.First(t => t.Key == link.Name!).Value);
	}

	public void ResetRegisters()
	{
		foreach (var register in InputRegisters) InputRegisters[register.Key] = default!;
	}

	[RelayCommand]
	public void AddRegister(string registerName)
	{
		if (Registers.All(t => t.Key != registerName))
		{
			Registers.Add(new ObservableKeyValuePair<string, double>(registerName, default!));
			InputRegisters[registerName] = default!;
			Operations[registerName] = inputRegisters => inputRegisters[registerName];
			
			(Parent as NodeViewModel)!.AddPin(0, 0, 10, 10, PinAlignment.Left, registerName, PinType.Input);
			(Parent as NodeViewModel)!.AddPin(0, 0, 10, 10, PinAlignment.Right, registerName, PinType.Output);
		}
	}
}