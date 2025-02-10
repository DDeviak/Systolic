using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations;

public class MulticastNode<T> : INode<T> where T : INumber<T>
{
	public Dictionary<string, IList<INode<T>>> Links { get; set; } = new();

	public void SetRegister(string registerName, T value)
	{
		foreach (var link in Links[registerName]) link.SetRegister(registerName, value);
	}
}