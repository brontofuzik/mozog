namespace Mozog.Search.Adversarial
{
    public interface IAdversarialSearch
    {
        IAction MakeDecision(IState state);
    }
}
