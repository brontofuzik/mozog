namespace Mozog.Search.Adversarial
{
    public interface IAdversarialSearch
    {
        Metrics Metrics { get; }

        (IAction move, double eval) MakeDecision(IState state);

        (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state);
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
