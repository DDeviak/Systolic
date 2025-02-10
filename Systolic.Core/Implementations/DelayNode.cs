using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations;

public class DelayNode<T> : INode<T> where T : INumber<T>
{
	public DelayNode(int delay)
	{
		Delay = delay;
	}

	public Dictionary<string, INode<T>> Links { get; set; } = new();

	protected Dictionary<string, Queue<T>> Buffers { get; set; } = new();

	protected int Delay { get; }

	public void SetRegister(string registerName, T value)
	{
		if (!Links.ContainsKey(registerName)) return;

		if (!Buffers.ContainsKey(registerName))
			Buffers[registerName] = new Queue<T>(Enumerable.Repeat(default(T)!, Delay));

		Buffers[registerName].Enqueue(value);
		Links[registerName].SetRegister(registerName, Buffers[registerName].Dequeue());
	}
}