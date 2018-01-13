using System;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class DepthLimitedSearch<S, A> : ISearchAlgo<S, A>
        where S : class
    {
        public const string NodesExpanded = "NodesExpanded";
	    public const string PathCost = "PathCost";

        private readonly int limit;
        private readonly NodeExpander<S, A> nodeExpander;
        public Node<S, A> cutoffNode = new Node<S, A>(null);

        public Metrics Metrics { get; } = new Metrics();

        public DepthLimitedSearch(int limit, NodeExpander<S, A> nodeExpander)
        {
            this.limit = limit;
            this.nodeExpander = nodeExpander ?? new NodeExpander<S, A>();
        }

        public S FindState(ISearchProblem<S, A> problem)
        {
            nodeExpander.UseParentLinks(false);
            Node<S, A> node = FindNode(problem);
            return !IsCutoffResult(node) ? ToState(node) : null;
        }

        public static S ToState(Node<S, A> node)
            => node != null ? node.State : null;

        public List<A> FindActions(ISearchProblem<S, A> problem)
        {
            nodeExpander.UseParentLinks(true);
            Node<S, A> node = FindNode(problem);
            return !IsCutoffResult(node) ? ToActions(node) : null;
        }

        public static List<A> ToActions(Node<S, A> node)
            => node != null ? GetActionSequence(node) : null;

        public static List<A> GetActionSequence(Node<S, A> node)
        {
            var actions = new List<A>();
            while (!node.IsRootNode)
            {
                actions.Add(node.Action);
                node = node.Parent;
            }
            return actions;
        }

        public Node<S, A> FindNode(ISearchProblem<S, A> problem)
        {
            ClearMetrics();

            var rootNode = nodeExpander.CreateRootNode(problem.InitialState);
            return RecursiveDLS(problem, rootNode, limit);
        }

        private Node<S, A> RecursiveDLS(ISearchProblem<S, A> problem, Node<S, A> node, int limit)
        {
            if (problem.TestGoal(node.State))
            {
                Metrics.Set(PathCost, node.PathCost);
                return node;
            }
            else if (0 == limit || ThreadingUtils.IsCurrentThreadCancelled())
            {
                return cutoffNode;
            }
            else
            {
                bool cutoffOccurred = false;

                Metrics.IncrementInt(NodesExpanded);
                foreach (var successor in nodeExpander.Expand(node, problem))
                {
                    var result = RecursiveDLS(problem, successor, limit - 1);

                    if (result == cutoffNode)
                        cutoffOccurred = true;
                    else if (result != null)
                        return result;
                }

                return cutoffOccurred ? cutoffNode : null;
            }
        }

        public bool IsCutoffResult(Node<S, A> node)
            => node != null && node == cutoffNode;

        public void AddNodeListener(Action<Node<S, A>> listener)
            => nodeExpander.AddNodeListener(listener);

        public bool RemoveNodeListener(Action<Node<S, A>> listener)
            => nodeExpander.RemoveNodeListener(listener);

        private void ClearMetrics()
        {
            Metrics.Set(NodesExpanded, 0);
            Metrics.Set(PathCost, 0);
        }
    }
}
