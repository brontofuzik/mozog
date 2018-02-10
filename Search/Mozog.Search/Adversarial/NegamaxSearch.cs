using System;
using Mozog.Utils;

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

            (double eval, IAction move) result = Negamax(state, color, prune, Double.MinValue, Double.MaxValue);

            return (result.move, result.eval);
        }

        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state)
        {
            var result = MakeDecision(state);
            return (result.move, result.eval, Metrics.Get<int>(NodesExpanded_Move));
        }

        private (double eval, IAction move) Negamax(IState state, int color, bool prune, double alpha, double beta)
        {
            var alphaOrig = alpha;

            if (transTable != null && TranspositionLookup(state, ref alpha, ref beta, out var cached))
                return cached;

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
                double eval = -Negamax(newState, -color, prune, -beta, -alpha).eval;

                // New best move found
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestAction = action;
                }

                // Alpha-beta pruning
                if (prune)
                {
                    alpha = Math.Max(alpha, eval);
                    if (alpha >= beta) break;
                }
            }

            if (transTable != null)
                TranspositionStore(state, bestEval, alphaOrig, beta, bestAction);

            return (bestEval, bestAction);
        }

        private bool TranspositionLookup(IState state, ref double alpha, ref double beta, out (double eval, IAction move) cached)
        {
            var ttEntry = transTable.Lookup(state);
            if (ttEntry.HasValue)
            {
                switch (ttEntry.Value.Flag)
                {
                    case TTFlag.Exact:
                    {
                        cached = (ttEntry.Value.Eval, ttEntry.Value.Action);
                        return true;
                    }
                    case TTFlag.LowerBound:
                        alpha = Math.Max(alpha, ttEntry.Value.Eval);
                        break;
                    case TTFlag.UpperBound:
                        beta = Math.Min(beta, ttEntry.Value.Eval);
                        break;
                }

                if (alpha >= beta)
                {
                    cached = (ttEntry.Value.Eval, ttEntry.Value.Action);
                    return true;
                }
            }

            cached = (0.0, null);
            return false;
        }

        private void TranspositionStore(IState state, double bestEval, double alphaOrig, double beta, IAction bestAction)
        {
            TTFlag flag;
            if (bestEval <= alphaOrig)
                flag = TTFlag.UpperBound;
            else if (bestEval >= beta)
                flag = TTFlag.LowerBound;
            else
                flag = TTFlag.Exact;
            var entry = new TTEntry { Eval = bestEval, Action = bestAction, Flag = flag };
            transTable.Store(state, entry);
        }
    }
}
