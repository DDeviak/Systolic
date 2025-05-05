using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Systolic.UI.Converters;

public class EnumStringConverter : IValueConverter
{
	public static readonly EnumStringConverter Instance = new();
	
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value?.ToString();
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return Enum.Parse(targetType, value?.ToString() ?? string.Empty);
	}
}