using System.Collections.Generic;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using Systolic.Core.Abstractions;
using Systolic.Core.Implementations;

namespace Systolic.UI.ViewModels.Overrides;

public enum PinType
{
	None,
	Input,
	Output
}

public class ExtendedPinViewModel : PinViewModel, INode<double>
{
	public INode<double>? Node { get; set; }

	public PinType PinType { get; set; }

	public void SetRegister(string registerName, double value)
	{
		if (PinType == PinType.None) return;
		if (PinType == PinType.Input) Node ??= Parent!.Content as INode<double>;

		Node?.SetRegister(registerName, value);
	}

	protected IPin GetOtherSide(IConnector connector)
	{
		return connector.Start == this ? connector.End : connector.Start;
	}

	public void Connect(IConnector connector)
	{
		if (PinType == PinType.Output && GetOtherSide(connector) is ExtendedPinViewModel otherSide)
		{
			if (Node is not MulticastNode<double>) Node = new MulticastNode<double>();

			var node = (Node as MulticastNode<double>)!;
			if (!node.Links.ContainsKey(Name!)) node.Links[Name!] = new List<INode<double>>();
			if (!node.Links[Name!].Contains(otherSide)) node.Links[Name!].Add(otherSide);
		}
	}

	public void Disconnect(IConnector connector)
	{
		if (PinType == PinType.Output && GetOtherSide(connector) is ExtendedPinViewModel otherSide)
		{
			var node = Node as MulticastNode<double>;
			var links = node?.Links[Name!];
			links?.Remove(otherSide);
		}
	}
}