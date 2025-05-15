using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Systolic.Core.Abstractions;
using Systolic.UI.ViewModels.Abstractions;

namespace Systolic.UI.ViewModels;

public partial class RunnerViewModel : ViewModelBase
{
	[ObservableProperty] IEnumerable<IProcessingNode<double>> _nodes;
	[ObservableProperty] IEnumerable<IInputProvider> _inputProviders;

	[ObservableProperty] private bool _isRunning;
	[ObservableProperty] private int _stepDelay = 100;
	
	private Task _runnerTask;
	
	[RelayCommand]
	public void Start()
	{
		IsRunning = true;
		_runnerTask = InnerTask();
	}
	
	[RelayCommand]
	public void Stop()
	{
		IsRunning = false;
		Task.Run(async () => await _runnerTask);
		_runnerTask = null;
	}
	
	[RelayCommand]
	public void Step()
	{	
		foreach (var node in Nodes)
		{
			node.ShiftRegisters();
		}
		
		foreach (var inputProvider in InputProviders)
		{
			inputProvider.ProvideInputs();
		}
			
		foreach (var node in Nodes)
		{
			node.PerformOperations();
		}
	}
	
	private async Task InnerTask()
	{
		while (IsRunning)
		{
			Step();
			await Task.Delay(StepDelay);
		}
	}
}