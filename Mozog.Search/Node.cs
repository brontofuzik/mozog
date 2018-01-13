namespace Mozog.Search
{
    // TODO Make this a struct
    public class Node<S, A>
    {
        public Node(S state)
        {
            State = state;
        }

        public Node(S state, Node<S, A> parent, A action, double pathCost)
            : this(state)
        {
            Parent = parent;
            Action = action;
            PathCost = pathCost;
        }

        public S State { get; }

        public Node<S, A> Parent { get; }

        public A Action { get; }

        public double PathCost { get; }

        public bool IsRootNode => Parent == null;
    }
}
