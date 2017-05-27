namespace AntColonyOptimization.Examples.FunctionOptimization
{
    internal class SphereModelFunction : ObjectiveFunction
    {
        public SphereModelFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps) => Mozog.Examples.Functions.SphereModel(steps);
    }
}
