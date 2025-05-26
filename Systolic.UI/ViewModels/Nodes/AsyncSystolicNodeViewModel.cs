using System.Collections.Generic;
using System.Linq;
using Systolic.UI.ViewModels.Abstractions;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Nodes;

public class AsyncSystolicNodeViewModel : BaseSystolicNodeViewModel, IAsyncSystolicNode
{
	Dictionary<string, double?> _registers = new();
	Dictionary<string, double> _registersToShift = new();

	int _counter = 0;

	public override void SetRegister(string registerName, double value)
	{
		if (!_registers.TryGetValue(registerName, out var r) || r is null ) return;
		
		_registers[registerName] = value;
		Registers.First(pair => pair.Name == registerName).Value = value;
		
		_counter++;
		
		if (_counter == Registers.Count)
		{
			_counter = 0;
			PerformOperations();
			foreach (var register in Registers)
				_registers[register.Name] = null;
			ShiftRegisters();
		}
	}

	public override void ShiftRegisters()
	{
		foreach (var link in Parent.Pins.OfType<ExtendedPinViewModel>().Where(t => t.PinType == PinType.Output))
			link.SetRegister(link.Name!, Registers.First(t => t.Name == link.Name!).Value);
	}

	public override void PerformOperations()
	{
		base.PerformOperations();

		foreach (var register in Registers)
		{
			register.Value =
				register.Operation.ExpressionFunc(_registers.ToDictionary(pair => pair.Key, pair => pair.Value ?? 0));
		}
	}

	public override void AddRegister(string? registerName)
	{
		base.AddRegister(registerName);
		_registers.Add(registerName, null);
		_registersToShift.Add(registerName, 0);
	}

	public override void RemoveRegister(string? registerName)
	{
		base.RemoveRegister(registerName);
		_registers.Remove(registerName);
		_registersToShift.Remove(registerName);
	}
}