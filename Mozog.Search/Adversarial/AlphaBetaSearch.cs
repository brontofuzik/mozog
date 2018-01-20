using System;

namespace Mozog.Search.Adversarial
{
    public class AlphaBetaSearch : IAdversarialSearch
    {
        private const string NodesExpanded_Game = "NodesExpanded_Game";
        private const string NodesExpanded_Move = "NodesExpanded_Move";

        private readonly IGame game;

        public Metrics Metrics { get; private set; } = new Metrics();

        public AlphaBetaSearch(IGame game)
        {
            Metrics.Set(NodesExpanded_Game, 0);
            this.game = game;
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);

            var (utility, action) = AlphaBeta(state, Double.MinValue, Double.MaxValue);
            return action;
        }

        //private double Minimax(IState state, Objective objective, string player, double alpha, double beta)
        //{
        //    Metrics.IncrementInt(NodesExpanded_Game);
        //    Metrics.IncrementInt(NodesExpanded_Move);

        //    if (game.IsTerminal(state))
        //        return game.GetUtility(state/*, player*/).Value;

        //    bool maximizing = objective == Objective.Max;
        //    double value = maximizing ? Double.MinValue : Double.MaxValue;
        //    Func<double, double, double> optimize = maximizing ? (Func<double, double, double>)Math.Max : (Func<double, double, double>)Math.Min;
        //    Objective opposite = maximizing ? Objective.Min : Objective.Max;

        //    foreach (IAction action in game.GetActions(state))
        //    {
        //        value = optimize(value, Minimax(game.GetResult(state, action), opposite, player, alpha, beta));

        //        // Prune search
        //        if (maximizing && value >= beta || !maximizing && value <= alpha)
        //            return value;

        //        // Update bounds
        //        if (maximizing)
        //            alpha = Math.Max(alpha, value);
        //        else
        //            beta = Math.Min(beta, value);
        //    }

        //    return value;
        //}

        private (double utility, IAction action) AlphaBeta(IState state, double alpha, double beta)
        {
            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (game.GetUtility(state).Value, null);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);
            var maximizing = objective == Objective.Max;
            var minimizing = objective == Objective.Min;

            IAction bestAction = null;
            var bestUtility = maximizing ? Double.MinValue : Double.MaxValue;

            foreach (var action in game.GetActions(state))
            {
                var newState = game.GetResult(state, action);
                var (newUtility, _) = AlphaBeta(newState, alpha, beta);

                if (maximizing && newUtility > bestUtility || minimizing && newUtility < bestUtility)
                {
                    bestAction = action;
                    bestUtility = newUtility;
                }

                // Prune search
                if (maximizing && newUtility >= beta || minimizing && newUtility <= alpha)
                    return (newUtility, action);

                // Update bounds
                if (maximizing)
                    alpha = Math.Max(alpha, newUtility);
                else
                    beta = Math.Min(beta, newUtility);
            }

            return (bestUtility, bestAction);
        }
    }
}
