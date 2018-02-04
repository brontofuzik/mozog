using System.Collections.Generic;
using Mozog.Utils;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        private readonly IDictionary<long, (double, IAction, bool)> tableWithMoves = new Dictionary<long, (double, IAction, bool)>();

        public void Store(IState state, double eval, IAction move, bool exact)
            => tableWithMoves[state.Hash] = (eval, move, exact);

        public (double eval, IAction action, bool exact)? Retrieve(IState state)
            => tableWithMoves.GetOrDefaultNullable(state.Hash);

        public void Clear_DEBUG()
        {
            tableWithMoves.Clear();
        }
    }

    public interface ITranspositionTable
    {
        void Store(IState state, double eval, IAction move, bool exact);

        (double eval, IAction action, bool exact)? Retrieve(IState state);

        void Clear_DEBUG();
    }
}
