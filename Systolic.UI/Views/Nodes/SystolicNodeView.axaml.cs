using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Systolic.UI.ViewModels.Abstractions;

namespace Systolic.UI.Views.Nodes;

public partial class SystolicNodeView : UserControl
{
	public SystolicNodeView()
	{
		InitializeComponent();
	}

	protected override void OnDataContextChanged(EventArgs e)
	{
		base.OnDataContextChanged(e);

		if (DataContext is BaseSystolicNodeViewModel viewModel)
		{
			viewModel.Operating += OnOperating;
		}
	}

	private void OnOperating()
	{
		var color = Rectangle.Fill;
		Rectangle.Fill = new SolidColorBrush(Colors.Aqua);
		Task.Run(() => 
		{
			Task.Delay(10).Wait();
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				Rectangle.Fill = color;
			});
		});
	}
}