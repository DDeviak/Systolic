using System.Numerics;

namespace Systolic.Core.Abstractions;

public interface INode<TNumber> where TNumber : INumber<TNumber>
{
	public void SetRegister(string registerName, TNumber value);
}