using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.Core.Abstractions;
using Systolic.UI.ViewModels.Nodes;

namespace Systolic.UI.ViewModels.Overrides;

public partial class ExtendedDrawingNodeViewModel : NodeViewModel, IDrawingNode
{
	private readonly DrawingNodeEditor _editor;
	[ObservableProperty] private RunnerViewModel _runnerViewModel;
	[ObservableProperty] private IList<IConnector>? _connectors;
	[ObservableProperty] private IList<INode>? _nodes;
	private ISet<IConnector>? _selectedConnectors;
	private ISet<INode>? _selectedNodes;
	private INodeSerializer? _serializer;
	[ObservableProperty] private IDrawingNodeSettings _settings;

	public ExtendedDrawingNodeViewModel() : this(DrawingNodeFactory.Instance)
	{
	}

	public ExtendedDrawingNodeViewModel(IDrawingNodeFactory drawingNodeFactory)
	{
		_editor = new DrawingNodeEditor(this, drawingNodeFactory);

		_settings = new DrawingNodeSettingsViewModel();
		
		RunnerViewModel = new RunnerViewModel();

		CutNodesCommand = new RelayCommand(CutNodes);

		CopyNodesCommand = new RelayCommand(CopyNodes);

		PasteNodesCommand = new RelayCommand(PasteNodes);

		DuplicateNodesCommand = new RelayCommand(DuplicateNodes);

		SelectAllNodesCommand = new RelayCommand(SelectAllNodes);

		DeselectAllNodesCommand = new RelayCommand(DeselectAllNodes);

		DeleteNodesCommand = new RelayCommand(DeleteNodes);
	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);
		
		if (e.PropertyName == nameof(Nodes))
		{
			RunnerViewModel.Nodes = Nodes.Select(t => t.Content).OfType<IProcessingNode<double>>();
			RunnerViewModel.InputProviders = Nodes.Select(t => t.Content).OfType<IInputProvider>();
		}
	}

	public event SelectionChangedEventHandler? SelectionChanged;

	public ICommand CutNodesCommand { get; }

	public ICommand CopyNodesCommand { get; }

	public ICommand PasteNodesCommand { get; }

	public ICommand DuplicateNodesCommand { get; }

	public ICommand SelectAllNodesCommand { get; }

	public ICommand DeselectAllNodesCommand { get; }

	public ICommand DeleteNodesCommand { get; }

	public void NotifySelectionChanged()
	{
		SelectionChanged?.Invoke(this, EventArgs.Empty);
	}

	public void NotifyDeselectedNodes()
	{
		var selectedNodes = GetSelectedNodes();
		if (selectedNodes is not null)
			foreach (var selectedNode in selectedNodes)
				selectedNode.OnDeselected();
	}

	public void NotifyDeselectedConnectors()
	{
		var selectedConnectors = GetSelectedConnectors();
		if (selectedConnectors is not null)
			foreach (var selectedConnector in selectedConnectors)
				selectedConnector.OnDeselected();
	}

	public ISet<INode>? GetSelectedNodes()
	{
		return _selectedNodes;
	}

	public void SetSelectedNodes(ISet<INode>? nodes)
	{
		_selectedNodes = nodes;
	}

	public ISet<IConnector>? GetSelectedConnectors()
	{
		return _selectedConnectors;
	}

	public void SetSelectedConnectors(ISet<IConnector>? connectors)
	{
		_selectedConnectors = connectors;
	}

	public INodeSerializer? GetSerializer()
	{
		return _serializer;
	}

	public void SetSerializer(INodeSerializer? serializer)
	{
		_serializer = serializer;
	}

	public T? Clone<T>(T source)
	{
		return _editor.Clone(source);
	}

	public bool IsPinConnected(IPin pin)
	{
		return _editor.IsPinConnected(pin);
	}

	public bool IsConnectorMoving()
	{
		return _editor.IsConnectorMoving();
	}

	public void CancelConnector()
	{
		_editor.CancelConnector();
	}

	public virtual bool CanSelectNodes()
	{
		return _editor.CanSelectNodes();
	}

	public virtual bool CanSelectConnectors()
	{
		return _editor.CanSelectConnectors();
	}

	public virtual bool CanConnectPin(IPin pin)
	{
		return _editor.CanConnectPin(pin) && !(pin is ExtendedPinViewModel { PinType: PinType.Input } extendedPin &&
		                                       Connectors.Any(c => c.Start == extendedPin || c.End == extendedPin));
	}

	public virtual void DrawingLeftPressed(double x, double y)
	{
		_editor.DrawingLeftPressed(x, y);
	}

	public virtual void DrawingRightPressed(double x, double y)
	{
		_editor.DrawingRightPressed(x, y);
	}

	public virtual void ConnectorLeftPressed(IPin pin, bool showWhenMoving)
	{
		_editor.ConnectorLeftPressed(pin, showWhenMoving);
	}

	public virtual void ConnectorMove(double x, double y)
	{
		_editor.ConnectorMove(x, y);
	}

	public virtual void CutNodes()
	{
		_editor.CutNodes();
	}

	public virtual void CopyNodes()
	{
		_editor.CopyNodes();
	}

	public virtual void PasteNodes()
	{
		_editor.PasteNodes();
	}

	public virtual void DuplicateNodes()
	{
		_editor.DuplicateNodes();
	}

	public virtual void DeleteNodes()
	{
		_editor.DeleteNodes();
	}

	public virtual void SelectAllNodes()
	{
		_editor.SelectAllNodes();
	}

	public virtual void DeselectAllNodes()
	{
		_editor.DeselectAllNodes();
	}

	public void Disconnect(ConnectorViewModel deletedConnector)
	{
		(deletedConnector.Start as ExtendedPinViewModel)?.Disconnect(deletedConnector);
		(deletedConnector.End as ExtendedPinViewModel)?.Disconnect(deletedConnector);
	}

	public void Connect(ConnectorViewModel newConnector)
	{
		(newConnector.Start as ExtendedPinViewModel)?.Connect(newConnector);
		(newConnector.End as ExtendedPinViewModel)?.Connect(newConnector);
	}
}