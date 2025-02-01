using System.Numerics;

namespace Systolic.Core.Abstractions
{
    public interface IProcessingNode<TNumber> : INode<TNumber> where TNumber : INumber<TNumber>
    {
        public void PerformOperations();
        public void ShiftRegisters();
    }
}