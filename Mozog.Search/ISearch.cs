namespace Mozog.Search
{
    public interface ISearch
    {
        IAction MakeDecision(IState state);
    }
}
