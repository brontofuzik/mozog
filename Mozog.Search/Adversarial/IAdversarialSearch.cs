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

    public static class ObjectiveExtensions
    {
        public static bool Max(this Objective objective)
            => objective == Objective.Max;

        public static bool Min(this Objective objective)
            => objective == Objective.Min;
    }
}
