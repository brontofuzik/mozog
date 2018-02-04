using System.Collections.Generic;
using Mozog.Utils;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        private readonly IDictionary<long, (double, IAction, TTFlag)> tableWithMoves = new Dictionary<long, (double, IAction, TTFlag)>();

        public void Store(IState state, double eval, IAction move, TTFlag flag)
            => tableWithMoves[state.Hash] = (eval, move, flag);

        public (double eval, IAction action, TTFlag flag)? Lookup(IState state)
            => tableWithMoves.GetOrDefaultNullable(state.Hash);

        public void Clear_DEBUG()
        {
            tableWithMoves.Clear();
        }
    }

    public interface ITranspositionTable
    {
        void Store(IState state, double eval, IAction move, TTFlag flag);

        (double eval, IAction action, TTFlag flag)? Lookup(IState state);

        void Clear_DEBUG();
    }

    public struct TTEntry
    {
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
