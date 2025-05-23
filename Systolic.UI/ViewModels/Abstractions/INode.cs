using System.Numerics;

namespace Systolic.UI.ViewModels.Abstractions;

public interface INode<TNumber> where TNumber : INumber<TNumber>
{
	public void SetRegister(string registerName, TNumber value);
}