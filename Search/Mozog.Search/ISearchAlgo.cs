using Mozog.Search.Problem;
using System;
using System.Collections.Generic;

namespace Mozog.Search
{
    public interface ISearchAlgo<S, A>
    {
        S FindState(ISearchProblem<S, A> problem);

        List<A> FindActions(ISearchProblem<S, A> problem);

        //Metrics getMetrics();

        void AddNodeListener(Action<Node<S, A>> listener);

        bool RemoveNodeListener(Action<Node<S, A>> listener);
    }
}
