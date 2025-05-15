using System.Linq;
using Systolic.UI.ViewModels.Abstractions;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public class SystolicNodeViewModel : BaseSystolicNodeViewModel, ISystolicNode
{
	public override void SetRegister(string registerName, double value)
	{
		Registers.First(pair => pair.Name == registerName).Value = value;
	}

	public override void PerformOperations()
	{
		base.PerformOperations();
		
		var inputRegisters = Registers.ToDictionary(pair => pair.Name, pair => pair.Value);
		foreach (var register in Registers) register.Value = register.Operation.ExpressionFunc(inputRegisters);
	}

	public override void ShiftRegisters()
	{
		foreach (var link in Parent.Pins.OfType<ExtendedPinViewModel>().Where(t => t.PinType == PinType.Output))
			link.SetRegister(link.Name!, Registers.First(t => t.Name == link.Name!).Value);
	}

	public void ResetRegisters()
	{
		foreach (var register in Registers) register.Value = default!;
	}

}