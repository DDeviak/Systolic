using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Systolic.Core.Abstractions;

namespace Systolic.UI.ViewModels.Nodes;

public partial class CollectorNodeViewModel : ViewModelBase, INode<double>
{
	[ObservableProperty]
	private ObservableCollection<Models.ObservableKeyValuePair<string, ObservableCollection<double>>> _registers = new();

	public void SetRegister(string registerName, double value)
	{
		if (Registers.All(t => t.Key != registerName))
		{
			var newCollection = new ObservableCollection<double>();
			var pair = new Models.ObservableKeyValuePair<string, ObservableCollection<double>>(registerName, newCollection);
			newCollection.CollectionChanged += (sender, args) => { Registers[Registers.IndexOf(pair)] = pair; };
			Registers.Add(pair);
		}

		Registers.First(t => t.Key == registerName).Value.Add(value);
	}
}