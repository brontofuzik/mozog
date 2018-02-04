﻿using System;

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
            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);

            // Transposition table
            var cached = transTable?.Retrieve(state);
            if (cached.HasValue)
            {
                if (cached.Value.exact)
                    return (cached.Value.eval, cached.Value.action);
                ImproveBounds(objective, cached.Value.eval, ref alpha, ref beta);
            }

            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (game.GetUtility(state).Value, null);

            IAction bestAction = null;
            var bestUtility = objective.Max() ? Double.MinValue : Double.MaxValue;
            bool exact = true;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double utility = Minimax(newState, prune, alpha, beta).utility;

                // New best move found
                if (Update(objective, utility, bestUtility))
                {
                    bestAction = action;
                    bestUtility = utility;
                }

                // Alpha-beta pruning
                if (prune && Prune(objective, bestUtility, ref alpha, ref beta))
                {
                    exact = false;
                    break;
                }
            }

            // Transposition table
            transTable?.Store(state, bestUtility, bestAction, exact);

            return (bestUtility, bestAction);
        }

        private static bool Update(Objective objective, double utility, double bestUtility)
            => objective.Max() && utility > bestUtility || objective.Min() && utility < bestUtility;

        private static bool Prune(Objective objective, double bestUtility, ref double alpha, ref double beta)
        {
            ImproveBounds(objective, bestUtility, ref alpha, ref beta);
            return alpha >= beta;
        }

        private static void ImproveBounds(Objective objective, double eval, ref double alpha, ref double beta)
        {
            if (objective.Max())
            {
                alpha = Math.Max(alpha, eval);
            }
            else // if (objective.Min())
            {
                beta = Math.Min(beta, eval);
            }
        }
    }
}
