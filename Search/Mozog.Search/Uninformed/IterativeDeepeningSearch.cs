using Mozog.Search.Problem;
using System;
using System.Collections.Generic;

namespace Mozog.Search.Uninformed
{
    public class IterativeDeepeningSearch<S, A> : ISearchAlgo<S, A>
        where S : class
    {
        public const string NodesExpanded = "NodesExpanded";
	    public const string PathCost = "PathCost";

	    private readonly NodeExpander<S, A> nodeExpander;

        public Metrics Metrics { get; } = new Metrics();

        public IterativeDeepeningSearch(NodeExpander<S, A> nodeExpander)
        {
            this.nodeExpander = nodeExpander ?? new NodeExpander<S, A>();
        }

        public S FindState(ISearchProblem<S, A> problem)
        {
            nodeExpander.UseParentLinks(false);
            return DepthLimitedSearch<S, A>.ToState(FindNode(problem));
        }

        public List<A> FindActions(ISearchProblem<S, A> problem)
        {
            nodeExpander.UseParentLinks(true);
            return DepthLimitedSearch<S, A>.ToActions(FindNode(problem));
        }

        private Node<S, A> FindNode(ISearchProblem<S, A> problem)
        {
            ClearMetrics();

            for (int i = 0; true; i++) // TODO
            {
                var dls = new DepthLimitedSearch<S, A>(i, nodeExpander);
                var result = dls.FindNode(problem);
                UpdateMetrics(dls.Metrics);
                if (!dls.IsCutoffResult(result))
                    return result;
            }

            return null;
        }

        #region Listeners

        public void AddNodeListener(Action<Node<S, A>> listener)
            => nodeExpander.AddNodeListener(listener);

        public bool RemoveNodeListener(Action<Node<S, A>> listener)
            => nodeExpander.RemoveNodeListener(listener);

        #endregion // Listeners

        private void UpdateMetrics(Metrics dlsMetrics)
        {
            Metrics.Set(NodesExpanded, Metrics.Get<int>(NodesExpanded) + dlsMetrics.Get<int>(NodesExpanded));
            Metrics.Set(PathCost, dlsMetrics.Get<double>(PathCost));
        }

        private void ClearMetrics()
        {
            Metrics.Set(NodesExpanded, 0);
            Metrics.Set(PathCost, 0);
        }
    }
}
