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

            IAction result = null;
            double resultValue = Double.MinValue;

            string player = game.GetPlayer(state);

            foreach (var action in game.GetActions(state))
            {
                double value = Minimax(game.GetResult(state, action), Objective.Min, player);

                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }

            return result;
        }

        private double Minimax(IState state, Objective objective, string player)
        {
            Metrics.IncrementInt(NodesExpanded);

            if (game.IsTerminal(state))
                return game.GetUtility(state, player).Value;

            bool maximizing = objective == Objective.Max;
            double value = maximizing ? Double.MinValue : Double.MaxValue;
            Func<double, double, double> optimize = maximizing ? (Func<double, double, double>)Math.Max : (Func<double, double, double>)Math.Min;
            Objective opposite = maximizing ? Objective.Min : Objective.Max;

            foreach (IAction action in game.GetActions(state))
                value = optimize(value, Minimax(game.GetResult(state, action), opposite, player));

            return value;
        }
    }

    internal enum Objective
    {
        Max,
        Min
    }
}
