using System;

namespace Mozog.Search.Adversarial
{
    public class IterativeDeepeningSearch : IAdversarialSearch
    {
        private readonly IGame game;
        private readonly IAdversarialSearch baseSearch;

        public IterativeDeepeningSearch(IGame game, IAdversarialSearch baseSearch)
        {
            this.game = game;
            this.baseSearch = baseSearch;
        }

        public static IAdversarialSearch New(IGame game, bool prune = true, bool tt = true)
        {
            var baseSearch = new MinimaxSearch(game, prune, tt);
            return new IterativeDeepeningSearch(game, baseSearch);
        }

        public Metrics Metrics { get; }

        public (IAction move, double eval) MakeDecision(IState state, int maxDepth = Int32.MaxValue)
        {
            IAction move = null;
            double eval = 0.0;

            int currentDepth = 0;
            while (currentDepth <= maxDepth)
            {
                (move, eval) = MakeDecision(state, currentDepth);
                currentDepth++;
            }

            return (move, eval);
        }

        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state, int maxDepth = Int32.MaxValue)
        {
            throw new NotImplementedException();
        }
    }
}
