using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozog.Search.Adversarial
{
    public class IterativeDeepeningAlphaBeta : IAdversarialSearch
    {
        public const string METRICS_NODES_EXPANDED = "NodesExpanded";
        public const string METRICS_MAX_DEPTH = "MaxDepth";

        protected IGame game;

        protected double utilMax;
        protected double utilMin;
        protected int currDepthLimit;

        // Indicates that non-terminal nodes have been evaluated.
        private bool heuristicEvaluationUsed; 
        private Timer timer;

        public Metrics Metrics { get; private set; }

        public IterativeDeepeningAlphaBeta(IGame game, double utilMin, double utilMax, int timeout)
        {
            this.game = game;
            this.utilMin = utilMin;
            this.utilMax = utilMax;
            this.timer = new Timer(timeout);
        }

        public IAction MakeDecision(IState state)
        {
            Metrics = new Metrics();
            timer.Start();

            currDepthLimit = 0;
            string player = game.GetPlayer(state);
            var candidateActions = OrderActions(state, game.GetActions(state), player, currDepthLimit);

            do
            {
                currDepthLimit++;
                heuristicEvaluationUsed = false;

                var refinedActions = new ActionStore();
                foreach (var action in candidateActions)
                {
                    if (timer.TimedOut) break;

                    double value = Minimax(Objective.Min, game.GetResult(state, action), player, Double.MinValue, Double.MaxValue, 1);
                    refinedActions.Add(action, value);
                }

                if (refinedActions.Size > 0)
                {
                    candidateActions = refinedActions.Actions;

                    if (!timer.TimedOut && (HasSafeWinner(refinedActions.Utilities[0])
                        || refinedActions.Size > 1 && IsSignificantlyBetter(refinedActions.Utilities[0], refinedActions.Utilities[1])))
                        break;
                }
            }
            while (!timer.TimedOut && heuristicEvaluationUsed);

            return candidateActions[0];
        }

        private double Minimax(Objective objective, IState state, string player, double alpha, double beta, int depth)
        {
            UpdateMetrics(depth);

            if (game.IsTerminal(state) || depth >= currDepthLimit || timer.TimedOut)
            {
                return Eval(state, player);
            }
            else
            {
                bool maximizing = objective == Objective.Max;
                double value = maximizing ? Double.MaxValue : Double.MinValue;
                Func<double, double, double> optimize = maximizing ? (Func<double, double, double>)Math.Max : (Func<double, double, double>)Math.Min;
                Objective opposite = maximizing ? Objective.Min : Objective.Max;

                foreach (var action in OrderActions(state, game.GetActions(state), player, depth))
                {
                    var value2 = Minimax(opposite, game.GetResult(state, action), player, alpha, beta, depth + 1);
                    value = optimize(value, value2);

                    // Prune search
                    if (maximizing && value >= beta || !maximizing && value <= alpha)
                        return value;

                    // Update bounds
                    if (maximizing)
                        alpha = Math.Max(alpha, value);
                    else
                        beta = Math.Min(beta, value);
                }

                return value;
            }
        }

        //private double MaxValue(S state, P player, double alpha, double beta, int depth)
        //{
        //    UpdateMetrics(depth);

        //    if (game.IsTerminal(state) || depth >= currDepthLimit || timer.TimedOut)
        //    {
        //        return Eval(state, player);
        //    }
        //    else
        //    {
        //        double value = Double.MinValue;
        //        foreach (var action in OrderActions(state, game.GetActions(state), player, depth))
        //        {
        //            var minValue = MinValue(game.GetResult(state, action), player, alpha, beta, depth + 1);
        //            value = Math.Max(value, minValue);

        //            if (value >= beta)
        //                return value;

        //            alpha = Math.Max(alpha, value);
        //        }
        //        return value;
        //    }
        //}

        //private double MinValue(S state, P player, double alpha, double beta, int depth)
        //{
        //    UpdateMetrics(depth);

        //    if (game.IsTerminal(state) || depth >= currDepthLimit || timer.TimedOut)
        //    {
        //        return Eval(state, player);
        //    }
        //    else
        //    {
        //        double value = Double.MaxValue;
        //        foreach (var action in OrderActions(state, game.GetActions(state), player, depth))
        //        {
        //            var maxValue = MaxValue(game.GetResult(state, action), player, alpha, beta, depth + 1);
        //            value = Math.Min(value, maxValue);

        //            if (value <= alpha)
        //                return value;

        //            beta = Math.Min(beta, value);
        //        }
        //        return value;
        //    }
        //}

        private void UpdateMetrics(int depth)
        {
            Metrics.IncrementInt(METRICS_NODES_EXPANDED);
            Metrics.Set(METRICS_MAX_DEPTH, Math.Max(Metrics.Get<int>(METRICS_MAX_DEPTH), depth));
        }

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a clear best action exists. This implementation returns
         * always false.
         */
        protected virtual bool IsSignificantlyBetter(double newUtility, double utility)
            => false;

        /**
         * Primitive operation which is used to stop iterative deepening search in
         * situations where a safe winner has been identified. This implementation
         * returns true if the given value (for the currently preferred action
         * result) is the highest or lowest utility value possible.
         */
        protected virtual bool HasSafeWinner(double resultUtility)
            => resultUtility <= utilMin || resultUtility >= utilMax;

        /**
         * Primitive operation, which estimates the value for (not necessarily
         * terminal) states. This implementation returns the utility value for
         * terminal states and <code>(utilMin + utilMax) / 2</code> for non-terminal
         * states. When overriding, first call the super implementation!
         */
        protected virtual double Eval(IState state, string player)
        {
            if (game.IsTerminal(state))
            {
                return game.GetUtility(state/*, player.ToString()*/).Value;
            }
            else
            {
                heuristicEvaluationUsed = true;
                return (utilMin + utilMax) / 2;
            }
        }

        /**
         * Primitive operation for action ordering. This implementation preserves
         * the original order (provided by the game).
         */
        protected virtual IList<IAction> OrderActions(IState state, IList<IAction> actions, string player, int depth)
        {
            return actions;
        }

        private class Timer
        {
            private TimeSpan duration;
            private DateTime startTime;

            public Timer(int maxSeconds)
            {
                duration = TimeSpan.FromSeconds(maxSeconds);
            }

            public void Start()
            {
                startTime = DateTime.Now;
            }

            public bool TimedOut => DateTime.Now > startTime.Add(duration);
        }

        // ???
        private class ActionStore
        {
            private List<(IAction a, double u)> actions = new List<(IAction, double)>();

            public void Add(IAction action, double utility)
            {
                int index = 0;
                while (index < actions.Count && utility <= actions[index].u) index++;
                actions.Insert(index, (action, utility));
            }

            public IList<IAction> Actions => actions.Select(p => p.a).ToList();

            public IList<double> Utilities => actions.Select(p => p.u).ToList();

            public int Size => actions.Count;
        }
    }
}
