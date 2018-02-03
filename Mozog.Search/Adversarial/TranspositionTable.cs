using System.Collections.Generic;
using Mozog.Utils;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        //private readonly Dictionary<long, double> tableWithEvals = new Dictionary<long, double>();
        private readonly IDictionary<long, (double, IAction, int)> tableWithMoves = new Dictionary<long, (double, IAction, int)>();

        //public double StoreEval(IState state, double eval)
        //    => tableWithEvals[state.Hash] = eval;

        public void Store(IState state, double eval, IAction move, int situation)
            => tableWithMoves[state.Hash] = (eval, move, situation);

        //public double? RetrieveEval(IState state)
        //    => tableWithEvals.GetOrDefaultNullable(state.Hash);

        public (double eval, IAction action, int situation)? Retrieve(IState state)
            => tableWithMoves.GetOrDefaultNullable(state.Hash);

        public void Clear_DEBUG()
        {
            //tableWithEvals.Clear();
            tableWithMoves.Clear();
        }
    }

    public interface ITranspositionTable
    {
        //double StoreEval(IState state, double eval);

        void Store(IState state, double eval, IAction move, int situation);

        //double? RetrieveEval(IState state);

        (double eval, IAction action, int situation)? Retrieve(IState state);

        void Clear_DEBUG();
    }
}
