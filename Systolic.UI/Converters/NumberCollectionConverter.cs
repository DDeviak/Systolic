using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Systolic.UI.Converters;

public class NumberCollectionConverter : IValueConverter
{
	public static NumberCollectionConverter Instance = new();

	public string Separator { get; set; } = ", ";

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is IList<double> numbers) return string.Join(Separator, numbers);

		return AvaloniaProperty.UnsetValue;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string text)
		{
			var parts = text.Split(Separator.Trim(), StringSplitOptions.RemoveEmptyEntries);
			var numbers = new ObservableCollection<double>();
			foreach (var part in parts)
				if (double.TryParse(part.Trim(), out var number))
					numbers.Add(number);

			return numbers;
		}

		return AvaloniaProperty.UnsetValue;
	}
}