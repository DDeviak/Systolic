using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NodeEditor.Model;
using Systolic.Core.Abstractions;

namespace Systolic.UI.ViewModels.Nodes;

public partial class SystolicNodeViewModel : ViewModelBase, IProcessingNode<double>
{
	private Dictionary<string, INode<double>> _links = null!;

	private Dictionary<string, Func<Dictionary<string, double>, double>> _operations = null!;

	[ObservableProperty] public IList<IPin> _pins;

	public SystolicNodeViewModel(IEnumerable<string> registerNames)
	{
		Registers = new Dictionary<string, double>();
		InputRegisters = new Dictionary<string, double>();
		foreach (var registerName in registerNames)
		{
			Registers[registerName] = default!;
			InputRegisters[registerName] = default!;
			Operations[registerName] = inputRegisters => inputRegisters[registerName];
		}
	}

	public ObservableCollection<KeyValuePair<string, double>> RegistersList => null!;

	protected Dictionary<string, double> Registers { get; }

	protected Dictionary<string, double> InputRegisters { get; }

	public Dictionary<string, Func<Dictionary<string, double>, double>> Operations
	{
		get => _operations ??= new Dictionary<string, Func<Dictionary<string, double>, double>>();
		set => _operations = value ?? new Dictionary<string, Func<Dictionary<string, double>, double>>();
	}

	public Dictionary<string, INode<double>> Links
	{
		get => _links ??= new Dictionary<string, INode<double>>();
		set => _links = value ?? new Dictionary<string, INode<double>>();
	}

	public void SetRegister(string registerName, double value)
	{
		InputRegisters[registerName] = value;
	}

	public void PerformOperations()
	{
		foreach (var operation in Operations) Registers[operation.Key] = operation.Value(InputRegisters);
	}

	public void ShiftRegisters()
	{
		foreach (var link in Links) link.Value?.SetRegister(link.Key, Registers[link.Key]);
	}

	public void ResetRegisters()
	{
		foreach (var register in InputRegisters) InputRegisters[register.Key] = default!;
	}

	public double GetRegister(string registerName)
	{
		return Registers[registerName];
	}
}