using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Systolic.UI.Services;

public class ExpressionParser
{
	public static Expression<Func<IDictionary<string, T>, T>> Parse<T>(string expression)
	{
		throw new NotImplementedException();
	}
}