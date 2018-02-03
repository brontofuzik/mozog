using System;

namespace Mozog.Search.Adversarial
{
    public class MinimaxSearch : IAdversarialSearch
    {
        public const string NodesExpanded_Game = "NodesExpanded_Game";
        public const string NodesExpanded_Move = "NodesExpanded_Move";

        private readonly IGame game;
        private readonly bool prune;
        private readonly ITranspositionTable transTable;

        public Metrics Metrics { get; } = new Metrics();

        public MinimaxSearch(IGame game, bool prune = true, bool tt = true)
        {
            this.game = game;
            this.prune = prune;
            transTable = tt ? new TranspositionTable() : null;

            Metrics.Set(NodesExpanded_Game, 0);
            Metrics.Set(NodesExpanded_Move, 0);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);
            transTable?.Clear_DEBUG();

            var alpha = prune ? Double.MinValue : 0.0;
            var beta = prune ? Double.MaxValue : 0.0;

            return Minimax(state, prune, alpha, beta).action;
        }

        // DEBUG: With nodes expanded per move
        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);
            transTable?.Clear_DEBUG();

            var alpha = prune ? Double.MinValue : 0.0;
            var beta = prune ? Double.MaxValue : 0.0;
            var r = Minimax(state, prune, alpha, beta);

            return (r.action, r.utility, Metrics.Get<int>(NodesExpanded_Move));
        }

        private (double utility, IAction action) Minimax(IState state, bool prune, double alpha, double beta)
        {
            // Transposition table
            var cached = transTable?.Retrieve(state);
            if (cached != null)
            {
                if (cached.Value.situation == 2)
                    return (cached.Value.eval, cached.Value.action);
                else if (cached.Value.situation == 1)
                {
                    // Improve beta
                    beta = Math.Min(beta, cached.Value.eval);
                }
                else if (cached.Value.situation == 3)
                {
                    // Improve alpha
                    alpha = Math.Max(alpha, cached.Value.eval);
                }
            }

            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (game.GetUtility(state).Value, null);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            IAction bestAction = null;
            var bestUtility = objective.Max() ? Double.MinValue : Double.MaxValue;
            int situation = 2; // Default

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double utility = Minimax(newState, prune, alpha, beta).utility;

                // New best move
                if (Update(objective, utility, bestUtility))
                {
                    bestAction = action;
                    bestUtility = utility;
                }

                if (prune && Prune(objective, bestUtility, ref alpha, ref beta))
                {
                    situation = objective.Min() ? 1 : 3;
                    break;
                }
            }

            // Transposition table
            transTable?.Store(state, bestUtility, bestAction, situation);

            return (bestUtility, bestAction);
        }

        private static bool Update(Objective objective, double utility, double bestUtility)
            => objective.Max() && utility > bestUtility || objective.Min() && utility < bestUtility;

        public bool Prune(Objective objective, double bestUtility, ref double alpha, ref double beta)
        {
            if (objective.Max())
            {
                alpha = Math.Max(alpha, bestUtility);
            }
            else // if (objective.Min())
            {
                beta = Math.Min(beta, bestUtility);
            }

            return alpha >= beta;
        }
    }
}
