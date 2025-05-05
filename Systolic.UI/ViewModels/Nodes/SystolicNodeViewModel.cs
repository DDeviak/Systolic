using System.Collections.ObjectModel;
using System.Linq;
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
	[ObservableProperty] private INode? _parent;

	[ObservableProperty] private ObservableCollection<Register> _registers =
		new();

	public void SetRegister(string registerName, double value)
	{
		Registers.First(pair => pair.Name == registerName).Value = value;
	}

	public void PerformOperations()
	{
		var inputRegisters = Registers.ToDictionary(pair => pair.Name, pair => pair.Value);
		foreach (var register in Registers) register.Value = register.Operation.ExpressionFunc(inputRegisters);
	}

	public void ShiftRegisters()
	{
		foreach (var link in Parent.Pins.OfType<ExtendedPinViewModel>().Where(t => t.PinType == PinType.Output))
			link.SetRegister(link.Name!, Registers.First(t => t.Name == link.Name!).Value);
	}

	public void ResetRegisters()
	{
		foreach (var register in Registers) register.Value = default!;
	}

	[RelayCommand]
	public void AddRegister(string registerName)
	{
		if (Registers.All(t => t.Name != registerName))
		{
			Registers.Add(new Register{
				Name = registerName, 
				Value = default!, 
				Operation = new Expression(registerName)
		});
			
			(Parent as NodeViewModel)!.AddPin(0, 0, 10, 10, PinAlignment.Left, registerName, PinType.Input);
			(Parent as NodeViewModel)!.AddPin(0, 0, 10, 10, PinAlignment.Right, registerName, PinType.Output);
		}
	}
}