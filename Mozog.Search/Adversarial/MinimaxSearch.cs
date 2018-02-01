using System;

namespace Mozog.Search.Adversarial
{
    public static class MinimaxSearch
    {
        public const string NodesExpanded_Game = "NodesExpanded_Game";
        public const string NodesExpanded_Move = "NodesExpanded_Move";

        public static MinimaxSearch<object> Default(IGame game, bool tt)
            => new MinimaxSearch<object>(game, new NoPruner(), tt);

        public static MinimaxSearch<(double alpha, double beta)> AlphaBeta(IGame game, bool tt)
            => new MinimaxSearch<(double alpha, double beta)>(game, new AlphaBetaPruner(), tt);
    }

    public class MinimaxSearch<TPrunerArgs> : IAdversarialSearch
    {
        private readonly IGame game;
        private readonly IPruner<TPrunerArgs> pruner;
        private readonly ITranspositionTable transTable;

        public Metrics Metrics { get; } = new Metrics();

        public MinimaxSearch(IGame game, IPruner<TPrunerArgs> pruner = null, bool tt = true)
        {
            this.game = game;
            this.pruner = pruner;
            transTable = tt ? new TranspositionTable() : null;

            Metrics.Set(MinimaxSearch.NodesExpanded_Game, 0);
            Metrics.Set(MinimaxSearch.NodesExpanded_Move, 0);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(MinimaxSearch.NodesExpanded_Move, 0);

            var (action, _) = Minimax(state, pruner.InitArgs);
            return action;
        }

        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state)
        {
            Metrics.Set(MinimaxSearch.NodesExpanded_Move, 0);

            transTable?.Clear_DEBUG();

            var r = Minimax(state, pruner.InitArgs);

            return (r.action, r.utility, Metrics.Get<int>(MinimaxSearch.NodesExpanded_Move));
        }

        private (IAction action, double utility) Minimax(IState state, TPrunerArgs prunerArgs)
        {
            // Transposition table
            var cached = transTable?.RetrieveEvalAndMove(state);
            if (cached != null) return (cached.Value.action, cached.Value.eval);

            Metrics.IncrementInt(MinimaxSearch.NodesExpanded_Game);
            Metrics.IncrementInt(MinimaxSearch.NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (null, game.GetUtility(state).Value);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            IAction bestAction = null;
            var bestUtility = objective.Max() ? Double.MinValue : Double.MaxValue;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double newUtility = Minimax(newState, prunerArgs).utility;

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

            // Transposition table
            transTable?.StoreEvalAndMove(state, bestUtility, bestAction);

            return (bestAction, bestUtility);
        }
    }
}
