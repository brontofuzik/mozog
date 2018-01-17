using System;

namespace Mozog.Search.Adversarial
{
    public class MinimaxSearch : IAdversarialSearch
    {
        private const string NodesExpanded = "NodesExpanded";

        private readonly IGame game;

        public Metrics Metrics { get; private set; }

        public MinimaxSearch(IGame game)
        {
            this.game = game;
        }

        public IAction MakeDecision(IState state)
        {
            Metrics = new Metrics();
            var (utility, action) = Minimax_NEW(state);
            return action;
        }

        //private double Minimax(IState state, Objective objective, string player)
        //{
        //    Metrics.IncrementInt(NodesExpanded);

        //    if (game.IsTerminal(state))
        //        return game.GetUtility(state, player).Value;

        //    bool maximizing = objective == Objective.Max;
        //    double value = maximizing ? Double.MinValue : Double.MaxValue;
        //    Func<double, double, double> optimize = maximizing ? (Func<double, double, double>)Math.Max : (Func<double, double, double>)Math.Min;
        //    Objective opposite = maximizing ? Objective.Min : Objective.Max;

        //    foreach (var action in game.GetActions(state))
        //        value = optimize(value, Minimax(game.GetResult(state, action), opposite, player));

        //    return value;
        //}

        private (double utility, IAction action) Minimax_NEW(IState state)
        {
            Metrics.IncrementInt(NodesExpanded);

            if (game.IsTerminal(state))
                return (game.GetUtility_NEW(state).Value, null);

            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            IAction bestAction = null;
            var bestUtility = objective == Objective.Max ? Double.MinValue : Double.MaxValue;

            foreach (var action in game.GetActions(state))
            {
                var newState = game.GetResult(state, action);
                var (newUtility, _) = Minimax_NEW(newState);

                if (objective == Objective.Max && newUtility > bestUtility
                    || objective == Objective.Min && newUtility < bestUtility)
                {
                    bestAction = action;
                    bestUtility = newUtility;
                }
            }

            return (bestUtility, bestAction);
        }
    }

    public enum Objective
    {
        Max,
        Min
    }
}
