using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Systolic.UI.ViewModels;
using Systolic.UI.ViewModels.Nodes;
using Systolic.UI.Views.Nodes;

namespace Systolic.UI;

public class ViewLocator : IDataTemplate
{
	Dictionary<Type,Type> _viewModelToView = new()
	{
		{ typeof(AsyncSystolicNodeViewModel), typeof(SystolicNodeView)}
	};
	public Control Build(object? data)
	{
		var name = data?.GetType().FullName?.Replace("ViewModel", "View");
		var type = name is null ? null : Type.GetType(name);

		if (type != null || _viewModelToView.TryGetValue(data.GetType(), out type)) return (Control)Activator.CreateInstance(type)!;

		return new TextBlock { Text = "Not Found: " + name };
	}

	public bool Match(object? data)
	{
		return data is ViewModelBase;
	}
}