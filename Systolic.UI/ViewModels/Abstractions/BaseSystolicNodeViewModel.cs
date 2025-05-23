using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.UI.Models;
using Systolic.UI.Services;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.ViewModels.Abstractions;

public abstract partial class BaseSystolicNodeViewModel : ViewModelBase, IProcessingNode<double>
{
	[ObservableProperty] private INode? _parent;

	[ObservableProperty] private ObservableCollection<Register> _registers =
		new();

	[JsonIgnore]
	public Action? Operating;
	
	private IRelayCommand<string>? _addRegisterCommand;
	public IRelayCommand<string> AddRegisterCommand =>
		_addRegisterCommand ??= new RelayCommand<string>(AddRegister);
	private IRelayCommand<string>? _removeRegisterCommand;
	public IRelayCommand<string> RemoveRegisterCommand =>
		_removeRegisterCommand ??= new RelayCommand<string>(RemoveRegister);
	
	public virtual void AddRegister(string? registerName)
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
	
	public virtual void RemoveRegister(string? registerName)
	{
		var register = Registers.FirstOrDefault(t => t.Name == registerName);
		if (register != null && Parent is NodeViewModel parent)
		{
			var pins = parent.Pins!.OfType<ExtendedPinViewModel>().Where(t => t.Name == registerName).ToList();
			foreach (var pin in pins)
			{
				parent.Pins.Remove(pin);
			}
			Registers.Remove(register);
		}
	}

	public abstract void SetRegister(string registerName, double value);

	public virtual void PerformOperations()
	{
		Operating?.Invoke();
	}

	public abstract void ShiftRegisters();
}