using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using Systolic.Core.Abstractions;

namespace Systolic.UI.ViewModels.Nodes;

public class CollectorNodeViewModel : ViewModelBase, INode<double>
{
	public AvaloniaDictionary<string, IList<double>> Registers { get; } = new();

	public void SetRegister(string registerName, double value)
	{
		if (!Registers.ContainsKey(registerName)) Registers[registerName] = new ObservableCollection<double>();
		Registers[registerName].Add(value);
	}
}