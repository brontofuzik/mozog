using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozog.Search.Adversarial
{
    public class NegamaxSearch : IAdversarialSearch
    {
        public const string NodesExpanded_Game = "NodesExpanded_Game";
        public const string NodesExpanded_Move = "NodesExpanded_Move";

        private readonly IGame game;
        private readonly bool prune;
        private readonly ITranspositionTable transTable;

        public Metrics Metrics { get; } = new Metrics();

        public NegamaxSearch(IGame game, bool prune = true, bool tt = true)
        {
            this.game = game;
            this.prune = prune;
            transTable = tt ? new TranspositionTable() : null;

            Metrics.Set(NodesExpanded_Game, 0);
            Metrics.Set(NodesExpanded_Move, 0);
        }

        public (IAction move, double eval) MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);
            transTable?.Clear();

            var objective = game.GetObjective(state.PlayerToMove);
            var color = objective.Max() ? 1 : -1;

            (double eval, IAction move) result;
            if (!prune)
            {
                result = Negamax(state, color);
            }
            else if (prune && transTable == null)
            {
                result = NegamaxWithPruning(state, color, Double.MinValue, Double.MaxValue);
            }
            else
            {
                result = NegamaxWithPruningAndTransposition(state, color, Double.MinValue, Double.MaxValue);
            }

            return (result.move, result.eval);
        }

        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state)
        {
            var result = MakeDecision(state);
            return (result.move, result.eval, Metrics.Get<int>(NodesExpanded_Move));
        }

        private (double eval, IAction move) Negamax(IState state, int color)
        {
            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (color * game.GetUtility(state).Value, null);

            var bestEval = Double.MinValue;
            IAction bestAction = null;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                var eval = -Negamax(newState, -color).eval;

                // New best move found
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestAction = action;
                }
            }

            return (bestEval, bestAction);
        }

        private (double eval, IAction move) NegamaxWithPruning(IState state, int color, double alpha, double beta)
        {
            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            // Terminal?
            if (game.IsTerminal(state))
                return (color * game.GetUtility(state).Value, null);

            var bestEval = Double.MinValue;
            IAction bestAction = null;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double eval = -NegamaxWithPruning(newState, -color, -beta, -alpha).eval;

                // New best move found
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestAction = action;
                }

                // Alpha-beta pruning
                alpha = Math.Max(alpha, eval);
                if (alpha >= beta) break;
            }

            return (bestEval, bestAction);
        }

        private (double eval, IAction move) NegamaxWithPruningAndTransposition(IState state, int color, double alpha, double beta)
        {
            var alphaOrig = alpha;

            // Transposition table lookup
            var ttEntry = transTable?.Lookup(state);
            if (ttEntry.HasValue)
            {
                switch (ttEntry.Value.Flag)
                {
                    case TTFlag.Exact:
                        return (ttEntry.Value.Eval, ttEntry.Value.Action); // Correct action?
                    case TTFlag.LowerBound:
                        alpha = Math.Max(alpha, ttEntry.Value.Eval);
                        break;
                    case TTFlag.UpperBound:
                        beta = Math.Min(beta, ttEntry.Value.Eval);
                        break;
                }

                if (alpha >= beta)
                    return (ttEntry.Value.Eval, ttEntry.Value.Action); // Correct action?
            }

            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            // Terminal?
            if (game.IsTerminal(state))
                return (color * game.GetUtility(state).Value, null);

            var bestEval = Double.MinValue;
            IAction bestAction = null;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double eval = -NegamaxWithPruning(newState, -color, -beta, -alpha).eval;

                // New best move found
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestAction = action;
                }

                // Alpha-beta pruning
                alpha = Math.Max(alpha, eval);
                if (alpha >= beta) break;
            }

            // Transposition table store
            TTFlag flag;
            if (bestEval <= alphaOrig)
                flag = TTFlag.UpperBound;
            else if (bestEval >= beta)
                flag = TTFlag.LowerBound;
            else
                flag = TTFlag.Exact;
            var entry = new TTEntry { Eval = bestEval, Action = bestAction, Flag = flag };
            transTable?.Store(state, entry);

            return (bestEval, bestAction);
        }
    }
}
