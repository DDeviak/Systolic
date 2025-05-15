using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.UI.ViewModels.Abstractions;
using Systolic.UI.ViewModels.Nodes;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.Services;

public class NodeFactory : INodeFactory
{
	public IDrawingNode CreateDrawing(string? name = null)
	{
		var settings = new DrawingNodeSettingsViewModel
		{
			EnableMultiplePinConnections = true,
			EnableSnap = true,
			SnapX = 15.0,
			SnapY = 15.0,
			EnableGrid = true,
			GridCellWidth = 15.0,
			GridCellHeight = 15.0
		};

		var factory = new DrawingNodeFactory();

		var connectors = new ObservableCollection<IConnector>();

		var drawing = new ExtendedDrawingNodeViewModel(factory)
		{
			Settings = settings,
			Name = name,
			X = 0,
			Y = 0,
			Width = 900,
			Height = 600,
			Nodes = new ObservableCollection<INode>(),
			Connectors = connectors
		};

		connectors.CollectionChanged += ConnectorsOnCollectionChanged;

		void ConnectorsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems?.Count > 0 && e.OldItems[0] is ConnectorViewModel deletedConnector)
			{
				drawing.Disconnect(deletedConnector);
				return;
			}

			if (e.NewItems is null) return;

			if (e.NewItems[0] is ConnectorViewModel newConnector)
				newConnector.PropertyChanged += (t, v) =>
				{
					switch (v.PropertyName)
					{
						case nameof(ConnectorViewModel.Start):
						case nameof(ConnectorViewModel.End):
							drawing.Connect(newConnector);
							break;
					}
				};
		}

		return drawing;
	}

	public IList<INodeTemplate> CreateTemplates()
	{
		return new ObservableCollection<INodeTemplate>
		{
			new NodeTemplateViewModel
			{
				Title = "Systolic Node",
				Template = CreateSystolicNode(0, 0),
				Preview = CreateSystolicNode(0, 0)
			},
			new NodeTemplateViewModel
			{
				Title = "Async Systolic Node",
				Template = CreateAsyncSystolicNode(0, 0),
				Preview = CreateAsyncSystolicNode(0, 0)
			},
			new NodeTemplateViewModel
			{
				Title = "Collector",
				Template = CreateCollectorNode(0, 0),
				Preview = CreateCollectorNode(0, 0)
			},
			new NodeTemplateViewModel
			{
				Title = "Input Source",
				Template = CreateInputSourceNode(0, 0),
				Preview = CreateInputSourceNode(0, 0)
			}
		};
	}

	internal static INode CreateSystolicNode(double x, double y, double width = 60, double height = 60,
		double pinSize = 10, string name = "PE")
	{
		var node = new NodeViewModel
		{
			Name = name,
			X = x,
			Y = y,
			Width = width,
			Height = height,
			Pins = new ObservableCollection<IPin>(),
			Content = new SystolicNodeViewModel()
		};

		(node.Content as SystolicNodeViewModel).Parent = node;

		return node;
	}
	
	internal static INode CreateAsyncSystolicNode(double x, double y, double width = 60, double height = 60,
		double pinSize = 10, string name = "PE")
	{
		var node = new NodeViewModel
		{
			Name = name,
			X = x,
			Y = y,
			Width = width,
			Height = height,
			Pins = new ObservableCollection<IPin>(),
			Content = new AsyncSystolicNodeViewModel()
		};

		(node.Content as BaseSystolicNodeViewModel).Parent = node;

		return node;
	}

	internal static INode CreateCollectorNode(double x, double y, double width = 120, double height = 60,
		double pinSize = 10, string name = "Collector")
	{
		var node = new NodeViewModel
		{
			Name = name,
			X = x,
			Y = y,
			Width = width,
			Height = height,
			Pins = new ObservableCollection<IPin>(),
			Content = new CollectorNodeViewModel()
		};

		node.AddPin(0, height / 2, pinSize, pinSize, PinAlignment.Left, "IN", PinType.Input);
		return node;
	}

	internal static INode CreateInputSourceNode(double x, double y, double width = 120, double height = 60,
		double pinSize = 10, string name = "Collector")
	{
		var node = new NodeViewModel
		{
			Name = name,
			X = x,
			Y = y,
			Width = width,
			Height = height,
			Pins = new ObservableCollection<IPin>(),
			Content = new InputSourceNodeViewModel()
		};

		(node.Content as InputSourceNodeViewModel).Parent = node;

		return node;
	}

	internal static IConnector CreateConnector(IPin? start, IPin? end)
	{
		return new ConnectorViewModel
		{
			Start = start,
			End = end
		};
	}
}