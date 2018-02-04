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

        public (IAction move, double eval) MakeDecision(IState state)
        {
            Metrics.Set(NodesExpanded_Move, 0);
            transTable?.Clear();

            var alpha = prune ? Double.MinValue : 0.0;
            var beta = prune ? Double.MaxValue : 0.0;
            var result = Minimax(state, prune, alpha, beta);

            return (result.action, result.utility);
        }

        public (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state)
        {
            var result = MakeDecision(state);
            return (result.move, result.eval, Metrics.Get<int>(NodesExpanded_Move));
        }

        private (double utility, IAction action) Minimax(IState state, bool prune, double alpha, double beta)
        {
            var alphaOrig = alpha;
            var betaOrig = beta;

            // Transposition table
            if (transTable != null && TranspositionLookup(state, ref alpha, ref beta, out var cached))
                return cached;

            Metrics.IncrementInt(NodesExpanded_Game);
            Metrics.IncrementInt(NodesExpanded_Move);

            if (game.IsTerminal(state))
                return (game.GetUtility(state).Value, null);

            // Maximizing or minimizing?
            string player = game.GetPlayer(state);
            var objective = game.GetObjective(player);
            var bestEval = objective.Max() ? Double.MinValue : Double.MaxValue;
            IAction bestAction = null;

            var moves = game.GetActionsAndResults(state);
            foreach (var (action, newState) in moves)
            {
                double utility = Minimax(newState, prune, alpha, beta).utility;

                // New best move found
                if (Update(objective, utility, bestEval))
                {
                    bestEval = utility;
                    bestAction = action;
                }

                // Alpha-beta pruning
                if (prune)
                {
                    if (objective.Max())
                        alpha = Math.Max(alpha, bestEval);
                    else // if (objective.Min())
                        beta = Math.Min(beta, bestEval);
                    if (alpha >= beta) break;
                }
            }

            if (transTable != null)
                TranspositionStore(state, bestEval, alphaOrig, betaOrig, bestAction);

            return (bestEval, bestAction);
        }

        private static bool Update(Objective objective, double eval, double bestEval)
            => objective.Max() && eval > bestEval || objective.Min() && eval < bestEval;

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

        private void TranspositionStore(IState state, double bestEval, double alphaOrig, double betaOrig, IAction bestAction)
        {
            TTFlag flag;
            if (bestEval <= alphaOrig)
                flag = TTFlag.UpperBound;
            else if (bestEval >= betaOrig)
                flag = TTFlag.LowerBound;
            else
                flag = TTFlag.Exact;

            var entry = new TTEntry { Eval = bestEval, Action = bestAction, Flag = flag };
            transTable.Store(state, entry);
        }
    }
}
