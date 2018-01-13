namespace Mozog.Search
{
    public interface ISearchProblem<S, A> : IOnlineSearchProblem<S, A>
    {
        S GetResult(S state, A action);
    }
}
