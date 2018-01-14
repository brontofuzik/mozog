using System.Collections.Generic;

namespace Mozog.Search.Problem
{
    public interface IOnlineSearchProblem<S, A>
    {
        S InitialState { get; }

        List<A> GetActions(S state);

        bool TestGoal(S state);

        double GetStepCosts(S state, A action, S stateDelta);
    }
}
