namespace AntColonyOptimization.Examples.FunctionOptimization
{
    internal class GoldsteinPriceFunction : ObjectiveFunction
    {
        public GoldsteinPriceFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps)
            => Mozog.Examples.Functions.GoldsteinPrice(steps);
    }
}
