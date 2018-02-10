using System.Collections.Generic;
using Mozog.Utils;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        private readonly IDictionary<long, TTEntry> tableWithMoves = new Dictionary<long, TTEntry>();

        public void Store(IState state, TTEntry entry)
            => tableWithMoves[state.Hash] = entry;

        public TTEntry? Lookup(IState state)
            => tableWithMoves.GetOrDefaultNullable(state.Hash);

        public void Clear()
        {
            tableWithMoves.Clear();
        }
    }

    public interface ITranspositionTable
    {
        void Store(IState state, TTEntry entry);

        TTEntry? Lookup(IState state);

        void Clear();
    }

    public struct TTEntry
    {
        public int Depth;

        public double Eval;

        public IAction Action;

        public TTFlag Flag;
    }

    public enum TTFlag
    {
        Exact,
        LowerBound,
        UpperBound
    }
}
