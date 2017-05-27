namespace AntColonyOptimization.Examples.FunctionOptimization
{
    internal class RosenbrockFunction : ObjectiveFunction
    {
        public RosenbrockFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps)
            => Mozog.Examples.Functions.Rosenbrock(steps);
    }
}
