using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations;

public class CollectorNode<TNumber> : INode<TNumber> where TNumber : INumber<TNumber>
{
	public Dictionary<string, IList<TNumber>> Registers { get; } = new();

	public void SetRegister(string registerName, TNumber value)
	{
		if (!Registers.ContainsKey(registerName)) Registers[registerName] = new List<TNumber>();
		Registers[registerName].Add(value);
	}
}