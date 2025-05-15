using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Systolic.UI.Models;

namespace Systolic.UI.Converters;

public class StringToExpressionConverter : IValueConverter
{
	public static StringToExpressionConverter Instance { get; } = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is Expression expression)
		{
			return expression.Infix;
		}

		return null;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string str)
		{
			try
			{
				return new Expression(str);
			}
			catch
			{
				//ignore
			}
		}

		return null;
	}
}