using System.Collections.Generic;

namespace Mozog.Search.Adversarial
{
    public class TranspositionTable : ITranspositionTable
    {
        private readonly Dictionary<long, double> table = new Dictionary<long, double>();

        public double StoreEvaluation(IState state, double eval)
            => table[state.Hash] = eval;

        public double? RetrieveEvaluation(IState state)
            => table.ContainsKey(state.Hash) ? table[state.Hash] : (double?) null;
    }

    public interface ITranspositionTable
    {
        double StoreEvaluation(IState state, double eval);

        double? RetrieveEvaluation(IState state);
    }
}
