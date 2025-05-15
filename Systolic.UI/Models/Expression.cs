using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Systolic.UI.Models;

public class Expression
{
	private string _infix = null!;
	private Func<IDictionary<string, double>, double>? _expressionFunc = null!; 
	
	public Expression(string infix)
	{
		Infix = infix;
	}

	[JsonIgnore]
	public Func<IDictionary<string, double>, double> ExpressionFunc
	{
		get => _expressionFunc ??= BuildExpression(_infix);
		set => _expressionFunc = value;
	}
	
	public string Infix
	{
		get => _infix; 
		set
		{
			_infix = value;
			ExpressionFunc = BuildExpression(_infix);
		}
	}
	
	public static IEnumerable<string> Tokenize(string infix)
	{
		string operators = "+-*/()";
		string currentToken = string.Empty;
		foreach (char c in infix)
		{
			if (char.IsWhiteSpace(c))
			{
				continue;
			}

			if (operators.Contains(c))
			{
				if (!string.IsNullOrEmpty(currentToken))
				{
					yield return currentToken;
					currentToken = string.Empty;
				}

				yield return c.ToString();
			}
			else
			{
				currentToken += c;
			}
		}
		if (!string.IsNullOrEmpty(currentToken))
		{
			yield return currentToken;
		}
	}

	public static IEnumerable<string> InfixToPostfix(string infix)
	{
		var output = new List<string>();
		var stack = new Stack<string>();
		var precedence = new Dictionary<string, int>
		{
			{ "+", 1 },
			{ "-", 1 },
			{ "*", 2 },
			{ "/", 2 }
		};

		foreach (var token in Tokenize(infix))
		{
			if (token == "(")
			{
				stack.Push(token);
			}
			else if (token == ")")
			{
				while (stack.Count > 0 && stack.Peek() != "(")
				{
					output.Add(stack.Pop());
				}

				stack.Pop();
			}
			else if (precedence.TryGetValue(token, out var tokenP))
			{
				while (stack.Count > 0 && precedence.TryGetValue(stack.Peek(), out var p) &&
				       p >= tokenP)
				{
					output.Add(stack.Pop());
				}

				stack.Push(token);
			}
			else
			{
				output.Add(token);
			}
		}

		while (stack.Count > 0)
		{
			output.Add(stack.Pop());
		}

		return output;
	}
	
	public static Func<IDictionary<string, double>, double> BuildExpression(string infix)
	{
		var operators = new HashSet<string> { "+", "-", "*", "/" };
		var postfix = InfixToPostfix(infix);
		var stack = new Stack<Func<IDictionary<string, double>, double>>();

		foreach (var token in postfix)
		{
			if (double.TryParse(token, out var number))
			{
				stack.Push(_ => number);
			}
			else if(operators.Contains(token))
			{
				var right = stack.Pop();
				var left = stack.Pop();

				stack.Push(token switch
				{
					"+" => (dict) => left(dict) + right(dict),
					"-" => (dict) => left(dict) - right(dict),
					"*" => (dict) => left(dict) * right(dict),
					"/" => (dict) => left(dict) / right(dict),
					_ => throw new InvalidOperationException($"Unknown operator: {token}")
				});
			}
			else
			{
				stack.Push(dict => dict[token]);
			}
		}

		return stack.Pop();
	}

	public override string ToString()
	{
		return Infix;
	}
	
	public static implicit operator Func<IDictionary<string, double>, double>(Expression expression)
	{
		return expression.ExpressionFunc;
	}
	
	public static implicit operator Expression(string infix)
	{
		return new Expression(infix);
	}
	
	public static implicit operator string(Expression expression)
	{
		return expression.Infix;
	}
}