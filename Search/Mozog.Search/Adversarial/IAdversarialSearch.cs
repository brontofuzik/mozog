using System;

namespace Mozog.Search.Adversarial
{
    public interface IAdversarialSearch
    {
        Metrics Metrics { get; }

        (IAction move, double eval) MakeDecision(IState state, int depth = Int32.MaxValue);

        (IAction move, double eval, int nodes) MakeDecision_DEBUG(IState state, int depth = Int32.MaxValue);
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
