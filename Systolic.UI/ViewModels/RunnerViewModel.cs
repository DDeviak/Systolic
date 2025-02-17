using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Systolic.Core.Abstractions;
using Systolic.UI.ViewModels.Nodes;

namespace Systolic.UI.ViewModels;

public partial class RunnerViewModel : ViewModelBase
{
	[ObservableProperty] ObservableCollection<IProcessingNode<double>> _nodes = new();
	[ObservableProperty] ObservableCollection<IInputProvider> _inputProviders = new();

	[ObservableProperty] private bool _isRunning;
	[ObservableProperty] private int _stepDelay = 100;
	
	private Task _runnerTask;
	
	[RelayCommand]
	public void Start()
	{
		_runnerTask = Task.Run(InnerTask);
	}
	
	[RelayCommand]
	public void Stop()
	{
		IsRunning = false;
		_runnerTask.Wait();
		_runnerTask = null;
	}
	
	[RelayCommand]
	public void Step()
	{
		foreach (var inputProvider in InputProviders)
		{
			inputProvider.ProvideInputs();
		}
			
		foreach (var node in Nodes)
		{
			node.PerformOperations();
		}
			
		foreach (var node in Nodes)
		{
			node.ShiftRegisters();
		}
	}
	
	private void InnerTask()
	{
		while (IsRunning)
		{
			Step();
			Task.Delay(StepDelay);
		}
	}
}