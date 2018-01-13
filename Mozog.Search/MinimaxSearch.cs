using System;

namespace Mozog.Search
{
    public class MinimaxSearch : IGameSearch
    {
        private const string NodesExpanded = "NodesExpanded";

        private readonly IGame game;

        private Metrics Metrics { get; } = new Metrics();

        public MinimaxSearch(IGame game)
        {
            this.game = game;
        }

        public IAction MakeDecision(IState state)
        {
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

        //private double MinValue(IState state, string player)
        //{
        //    //metrics.IncrementInt(NodesExpanded);

        //    if (game.IsTerminal(state))
        //        return game.GetUtility(state, player).Value;

        //    double value = Double.MaxValue;
        //    foreach (IAction action in game.GetActions(state))
        //        value = Math.Min(value, MaxValue(game.GetResult(state, action), player));

        //    return value;
        //}

        //private double MaxValue(IState state, string player)
        //{
        //    //metrics.IncrementInt(NodesExpanded);

        //    if (game.IsTerminal(state))
        //        return game.GetUtility(state, player).Value;

        //    double value = Double.MinValue;
        //    foreach (IAction action in game.GetActions(state))
        //        value = Math.Max(value, MinValue(game.GetResult(state, action), player));
        //    return value;
        //}
    }

    internal enum Objective
    {
        Max,
        Min
    }
}
