using System.Numerics;
using Systolic.Core.Abstractions;

namespace Systolic.Core.Implementations
{
    public class Node<TNumber> : IProcessingNode<TNumber> where TNumber : INumber<TNumber>
    {
        public Action OnTick { get; set; } = null!;

        protected Dictionary<string, TNumber> _registers;
        protected Dictionary<string, TNumber> _inputRegisters;

        public Dictionary<string, Func<Dictionary<string, TNumber>, TNumber>> Operations { get; set; } = new();

        public Dictionary<string, INode<TNumber>> Links { get; set; } = new();

        public Barrier Barrier { get; set; } = null!;

        public Node(IEnumerable<string> registerNames)
        {
            _registers = new Dictionary<string, TNumber>();
            _inputRegisters = new Dictionary<string, TNumber>();
            foreach (var registerName in registerNames)
            {
                _registers[registerName] = default!;
                _inputRegisters[registerName] = default!;
            }
        }

        public void SetRegister(string registerName, TNumber value)
        {
            _inputRegisters[registerName] = value;
        }

        public void PerformOperations()
        {
            foreach (var operation in Operations)
            {
                _registers[operation.Key] = operation.Value(_inputRegisters);
            }
        }

        public void ShiftRegisters()
        {
            foreach (var link in Links)
            {
                link.Value?.SetRegister(link.Key, _registers[link.Key]);
            }
        }

        public void ResetRegisters()
        {
            foreach (var register in _inputRegisters)
            {
                _inputRegisters[register.Key] = default!;
            }
        }

        public void Tick()
        {
            PerformOperations();
            Barrier?.SignalAndWait();
            ShiftRegisters();
        }

        public TNumber GetRegister(string registerName)
        {
            return _registers[registerName];
        }
    }
}