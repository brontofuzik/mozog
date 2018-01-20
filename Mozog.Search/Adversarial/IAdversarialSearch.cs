namespace Mozog.Search.Adversarial
{
    public interface IAdversarialSearch
    {
        IAction MakeDecision(IState state);

        Metrics Metrics { get; }
    }

    public enum Objective
    {
        Max,
        Min
    }
}
