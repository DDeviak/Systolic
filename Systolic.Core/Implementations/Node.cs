using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations;

public class Node<TNumber> : IProcessingNode<TNumber> where TNumber : INumber<TNumber>
{
	private Dictionary<string, INode<TNumber>> _links = null!;

	private Dictionary<string, Func<Dictionary<string, TNumber>, TNumber>> _operations = null!;

	public Node(IEnumerable<string> registerNames)
	{
		Registers = new Dictionary<string, TNumber>();
		InputRegisters = new Dictionary<string, TNumber>();
		foreach (var registerName in registerNames)
		{
			Registers[registerName] = default!;
			InputRegisters[registerName] = default!;
			Operations[registerName] = inputRegisters => inputRegisters[registerName];
		}
	}

	protected Dictionary<string, TNumber> Registers { get; }

	protected Dictionary<string, TNumber> InputRegisters { get; }

	public Dictionary<string, Func<Dictionary<string, TNumber>, TNumber>> Operations
	{
		get => _operations ??= new Dictionary<string, Func<Dictionary<string, TNumber>, TNumber>>();
		set => _operations = value ?? new Dictionary<string, Func<Dictionary<string, TNumber>, TNumber>>();
	}

	public Dictionary<string, INode<TNumber>> Links
	{
		get => _links ??= new Dictionary<string, INode<TNumber>>();
		set => _links = value ?? new Dictionary<string, INode<TNumber>>();
	}

	public void SetRegister(string registerName, TNumber value)
	{
		InputRegisters[registerName] = value;
	}

	public void PerformOperations()
	{
		foreach (var operation in Operations) Registers[operation.Key] = operation.Value(InputRegisters);
	}

	public void ShiftRegisters()
	{
		foreach (var link in Links) link.Value?.SetRegister(link.Key, Registers[link.Key]);
	}

	public void ResetRegisters()
	{
		foreach (var register in InputRegisters) InputRegisters[register.Key] = default!;
	}

	public TNumber GetRegister(string registerName)
	{
		return Registers[registerName];
	}
}