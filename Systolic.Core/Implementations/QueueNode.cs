using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations
{
    public class QueueNode<TNumber> : INode<TNumber> where TNumber : INumber<TNumber>
    {
        public Action OnTick { get; set; } = null!;

        public Dictionary<string, List<TNumber>> Registers { get; set; } = null!;
        public QueueNode(IEnumerable<string> registerNames)
        {
            Registers = new Dictionary<string, List<TNumber>>();
            foreach (var registerName in registerNames)
            {
                Registers[registerName] = new List<TNumber>();
            }
        }

        public void SetRegister(string registerName, TNumber value)
        {
            Registers[registerName].Add(value);
        }
    }
}