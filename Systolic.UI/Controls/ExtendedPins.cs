using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using NodeEditor.Controls;
using NodeEditor.Model;
using Systolic.UI.ViewModels.Overrides;

namespace Systolic.UI.Controls;

public class ExtendedPins : Pins
{
	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		
		if (change.Property == NodeSourceProperty)
		{
			if (change.OldValue is INode { Pins: ObservableCollection<IPin> oldPins })
			{
				oldPins.CollectionChanged -= Pins_CollectionChanged;
			}
			if (change.NewValue is INode { Pins: ObservableCollection<IPin> newPins })
			{
				newPins.CollectionChanged += Pins_CollectionChanged;
				foreach (var pin in newPins)
				{
					if (pin is ExtendedPinViewModel extendedPin)
					{
						extendedPin.PropertyChanged -= Pin_PropertyChanged;
						extendedPin.PropertyChanged += Pin_PropertyChanged;
					}
				}
			}
		}
	}

	private void Pins_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems != null)
		{
			foreach (var item in e.OldItems)
			{
				if (item is ExtendedPinViewModel pin)
				{
					pin.PropertyChanged -= Pin_PropertyChanged;
				}
			}
		}
		if (e.NewItems != null)
		{
			foreach (var item in e.NewItems)
			{
				if (item is ExtendedPinViewModel pin)
				{
					pin.PropertyChanged += Pin_PropertyChanged;
					RearrangePins();
				}
			}
		}
	}

	private void Pin_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is ExtendedPinViewModel pin && e.PropertyName == nameof(ExtendedPinViewModel.Alignment))
		{
			RearrangePins();
		}
	}

	private void RearrangePins()
	{
		var pinsGroups = NodeSource.Pins.GroupBy(p => p.Alignment);
		foreach (var group in pinsGroups)
		{
			var gap = group.Key == PinAlignment.Left || group.Key == PinAlignment.Right
				? NodeSource.Height / (group.Count() + 1)
				: NodeSource.Width / (group.Count() + 1);
			
			int index = 1;
			foreach (var pin in group.OrderBy(p=>p.Name))
			{
				pin.X = group.Key switch
				{
					PinAlignment.Left => 0,
					PinAlignment.Right => NodeSource.Width,
					_ => gap * index
				};
				
				pin.Y = group.Key switch
				{
					PinAlignment.Top => 0,
					PinAlignment.Bottom => NodeSource.Height,
					_ => gap * index
				};
				
				index++;
			}
		}

		NodeSource.Pins = NodeSource.Pins;
	}
}