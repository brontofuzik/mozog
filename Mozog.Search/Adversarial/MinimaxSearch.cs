using System;

namespace Mozog.Search.Adversarial
{
    public static class MinimaxSearch
    {
        public static MinimaxSearch<object> Default(IGame game)
            => new MinimaxSearch<object>(game);

        public static MinimaxSearch<(double alpha, double beta)> AlphaBeta(IGame game)
            => new MinimaxSearch<(double alpha, double beta)>(game, new AlphaBetaPruner());
    }

    public class MinimaxSearch<TPrunerArgs> : IAdversarialSearch
    {
        private const string NodesExpanded_Game = "NodesExpanded_Game";
        private const string NodesExpanded_Move = "NodesExpanded_Move";

        private readonly IGame game;
        private readonly IPruner<TPrunerArgs> pruner;

        private readonly ITranspositionTable transTable; //= new TranspositionTable();

        public Metrics Metrics { get; } = new Metrics();

        public MinimaxSearch(IGame game, IPruner<TPrunerArgs> pruner = null)
        {
            this.game = game;
            this.pruner = pruner;
            Metrics.Set(NodesExpanded_Game, 0);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);

            var (action, _) = Minimax_NEW(state, pruner.InitArgs);
            return action;
        }

        private (IAction action, double utility) Minimax_NEW(IState state, TPrunerArgs prunerArgs)
        {
            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (null, game.GetUtility(state).Value);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            IAction bestAction = null;
            var bestUtility = objective.Max() ? Double.MinValue : Double.MaxValue;

            foreach (var action in game.GetActions(state))
            {
                var newState = game.GetResult(state, action);

                var newUtility = transTable != null
                    ? (transTable.RetrieveEvaluation(newState) ?? transTable.StoreEvaluation(newState, Minimax_NEW(newState, prunerArgs).utility))
                    : Minimax_NEW(newState, prunerArgs).utility;

                // New best move
                if (objective.Max() && newUtility > bestUtility || objective.Min() && newUtility < bestUtility)
                {
                    bestAction = action;
                    bestUtility = newUtility;
                }

                // Prune
                if (pruner?.Prune(objective, newUtility, ref prunerArgs) ?? false)
                    return (action, newUtility);
            }

            return (bestAction, bestUtility);
        }
    }
}
