using System.Collections.Generic;
using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.Services;

public class DrawingNodeFactory : IDrawingNodeFactory
{
	public IPin CreatePin()
	{
		return new ExtendedPinViewModel();
	}

	public IConnector CreateConnector()
	{
		return new ConnectorViewModel();
	}

	public IList<T> CreateList<T>()
	{
		return new ObservableCollection<T>();
	}
}