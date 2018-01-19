using System;

namespace Mozog.Search.Adversarial
{
    public class AlphaBetaSearch : IAdversarialSearch
    {
        private const string NodesExpanded = "NodesExpanded";

        private readonly IGame game;

        public Metrics Metrics { get; private set; }

        public AlphaBetaSearch(IGame game)
        {
            this.game = game;
        }

        public IAction MakeDecision(IState state)
        {
            Metrics = new Metrics();

            IAction result = null;
            double resultValue = Double.MinValue;

            string player = game.GetPlayer(state);

            foreach (IAction action in game.GetActions(state))
            {
                double value = Minimax(game.GetResult(state, action), Objective.Min, player, Double.MinValue, Double.MaxValue);

                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }

            return result;
        }

        private double Minimax(IState state, Objective objective, string player, double alpha, double beta)
        {
            Metrics.IncrementInt(NodesExpanded);

            if (game.IsTerminal(state))
                return game.GetUtility(state/*, player*/).Value;

            bool maximizing = objective == Objective.Max;
            double value = maximizing ? Double.MinValue : Double.MaxValue;
            Func<double, double, double> optimize = maximizing ? (Func<double, double, double>)Math.Max : (Func<double, double, double>)Math.Min;
            Objective opposite = maximizing ? Objective.Min : Objective.Max;

            foreach (IAction action in game.GetActions(state))
            {
                value = optimize(value, Minimax(game.GetResult(state, action), opposite, player, alpha, beta));

                // Prune search
                if (maximizing && value >= beta || !maximizing && value <= alpha)
                    return value;

                // Update bounds
                if (maximizing)
                    alpha = Math.Max(alpha, value);
                else
                    beta = Math.Min(beta, value);
            }

            return value;
        }
    }
}
