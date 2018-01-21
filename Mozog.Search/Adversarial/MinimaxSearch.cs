using System;

namespace Mozog.Search.Adversarial
{
    public class MinimaxSearch : IAdversarialSearch
    {
        private const string NodesExpanded_Game = "NodesExpanded_Game";
        private const string NodesExpanded_Move = "NodesExpanded_Move";

        private readonly IGame game;

        public Metrics Metrics { get; private set; } = new Metrics();

        public MinimaxSearch(IGame game)
        {
            this.game = game;
            Metrics.Set(NodesExpanded_Game, 0);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);

            var (action, _) = Minimax(state);
            return action;
        }

        private (IAction action, double utility) Minimax(IState state)
        {
            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (null, game.GetUtility(state).Value);

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
                var (_, newUtility) = Minimax(newState);

                if (maximizing && newUtility > bestUtility || minimizing && newUtility < bestUtility)
                {
                    bestAction = action;
                    bestUtility = newUtility;
                }
            }

            return (bestAction, bestUtility);
        }
    }
}
