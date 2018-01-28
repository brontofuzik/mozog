using System;

namespace Mozog.Search.Adversarial
{
    public interface IPruner<TPrunerArgs>
    {
        TPrunerArgs InitArgs { get; }

        bool Prune(Objective objective, double utility, ref TPrunerArgs args);
    }

    public class NoPruner : IPruner<object>
    {
        public object InitArgs => new object();

        public bool Prune(Objective objective, double utility, ref object args) => false;
    }

    public class AlphaBetaPruner : IPruner<(double alpha, double beta)>
    {
        public (double alpha, double beta) InitArgs => (Double.MinValue, Double.MaxValue);

        public bool Prune(Objective objective, double utility, ref (double alpha, double beta) args)
        {
            // Prune search
            if (objective.Max() && utility >= args.beta || objective.Min() && utility <= args.alpha)
                return true;

            // Update bounds
            args = objective.Max()
                ? (Math.Max(args.alpha, utility), args.beta)
                : (args.alpha, Math.Min(args.beta, utility));

            return false;
        }
    }
}
