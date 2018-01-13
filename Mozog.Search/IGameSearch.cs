namespace Mozog.Search
{
    public interface IGameSearch
    {
        IAction MakeDecision(IState state);
    }
}
