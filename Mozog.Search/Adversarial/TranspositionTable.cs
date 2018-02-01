using System.Collections.Generic;
using Mozog.Utils;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        private readonly Dictionary<long, double> tableWithEvals = new Dictionary<long, double>();
        private readonly IDictionary<long, (double, IAction)> tableWithMoves = new Dictionary<long, (double, IAction)>();

        public double StoreEval(IState state, double eval)
            => tableWithEvals[state.Hash] = eval;

        public void StoreEvalAndMove(IState state, double eval, IAction move)
            => tableWithMoves[state.Hash] = (eval, move);

        public double? RetrieveEval(IState state)
            => tableWithEvals.GetOrDefaultNullable(state.Hash);

        public (double eval, IAction action)? RetrieveEvalAndMove(IState state)
            => tableWithMoves.GetOrDefaultNullable(state.Hash);

        public void Clear_DEBUG()
        {
            tableWithEvals.Clear();
            tableWithMoves.Clear();
        }
    }

    public interface ITranspositionTable
    {
        double StoreEval(IState state, double eval);

        void StoreEvalAndMove(IState state, double eval, IAction move);

        double? RetrieveEval(IState state);

        (double eval, IAction action)? RetrieveEvalAndMove(IState state);

        void Clear_DEBUG();
    }
}
