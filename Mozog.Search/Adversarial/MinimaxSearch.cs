using System;

namespace Mozog.Search.Adversarial
{
    public static class MinimaxSearch
    {
        public const string NodesExpanded_Game = "NodesExpanded_Game";
        public const string NodesExpanded_Move = "NodesExpanded_Move";

        public static MinimaxSearch<object> Default(IGame game)
            => new MinimaxSearch<object>(game, new NoPruner());

        public static MinimaxSearch<(double alpha, double beta)> AlphaBeta(IGame game)
            => new MinimaxSearch<(double alpha, double beta)>(game, new AlphaBetaPruner());
    }

    public class MinimaxSearch<TPrunerArgs> : IAdversarialSearch
    {
        private readonly IGame game;
        private readonly IPruner<TPrunerArgs> pruner;

        private readonly ITranspositionTable transTable = new TranspositionTable();

        public Metrics Metrics { get; } = new Metrics();

        public MinimaxSearch(IGame game, IPruner<TPrunerArgs> pruner = null)
        {
            this.game = game;
            this.pruner = pruner;

            Metrics.Set(MinimaxSearch.NodesExpanded_Game, 0);
            Metrics.Set(MinimaxSearch.NodesExpanded_Move, 0);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(MinimaxSearch.NodesExpanded_Move, 0);

            var (action, _) = Minimax_NEW(state, pruner.InitArgs);
            return action;
        }

        private (IAction action, double utility) Minimax_NEW(IState state, TPrunerArgs prunerArgs)
        {
            Metrics.IncrementInt(MinimaxSearch.NodesExpanded_Game);
            Metrics.IncrementInt(MinimaxSearch.NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (null, game.GetUtility(state).Value);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            IAction bestAction = null;
            var bestUtility = objective.Max() ? Double.MinValue : Double.MaxValue;

            foreach (var (action, newState) in game.GetActionsAndResults(state))
            {
                double newUtility;
                //if (transTable != null)
                //{
                    var tmp1 = transTable.RetrieveEvaluation(newState);
                    if (tmp1.HasValue)
                    {
                        newUtility = tmp1.Value;
                    }
                    else
                    {
                        var tmp2 = Minimax_NEW(newState, prunerArgs).utility;
                        transTable.StoreEvaluation(newState, tmp2);
                        newUtility = tmp2;
                    }
                //}
                //else
                //    newUtility = Minimax_NEW(newState, prunerArgs).utility;

                // New best move
                if (objective.Max() && newUtility > bestUtility || objective.Min() && newUtility < bestUtility)
                {
                    bestAction = action;
                    bestUtility = newUtility;
                }

                // Prune
                if (pruner.Prune(objective, newUtility, ref prunerArgs))
                    return (action, newUtility);
            }

            return (bestAction, bestUtility);
        }
    }
}
