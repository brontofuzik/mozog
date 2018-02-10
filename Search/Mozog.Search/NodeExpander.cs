using Mozog.Search.Problem;
using System;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class NodeExpander<S, A>
    {
        protected bool useParentLinks = true;

        public NodeExpander<S, A> UseParentLinks(bool s)
        {
            useParentLinks = s;
            return this;
        }

        #region Node expansion

        public Node<S, A> CreateRootNode(S state)
            => new Node<S, A>(state);

        public Node<S, A> CreateNode(S state, Node<S, A> parent, A action, double stepCost)
        {
            parent = useParentLinks ? parent : null;
            return new Node<S, A>(state, parent, action, parent.PathCost + stepCost);
        }

        public List<Node<S, A>> Expand(Node<S, A> node, ISearchProblem<S, A> problem)
        {
            var successors = new List<Node<S, A>>();

            foreach (var action in problem.GetActions(node.State))
            {
                var successorState = problem.GetResult(node.State, action);
                double stepCost = problem.GetStepCosts(node.State, action, successorState);
                var successorNode = CreateNode(successorState, node, action, stepCost);
                successors.Add(successorNode);
            }

            NotifyNodeListeners(node);

            return successors;
        }

        #endregion // Node expansion

        #region Progress tracking

        private List<Action<Node<S, A>>> nodeListeners = new List<Action<Node<S, A>>>();

        public void AddNodeListener(Action<Node<S, A>> listener)
            => nodeListeners.Add(listener);

        public bool RemoveNodeListener(Action<Node<S, A>> listener)
            => nodeListeners.Remove(listener);

        protected void NotifyNodeListeners(Node<S, A> node)
        {
            foreach (var listener in nodeListeners)
                listener(node);
        }

        #endregion // Progress tracking
    }
}
