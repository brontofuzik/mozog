namespace AntColonyOptimization.Examples
{
    static class FunctionOptimization
    {
        public static AntColonyOptimization SphereModelFunction
            => new AntColonyOptimization(6) { Objective = new SphereModel(6) };

        public static AntColonyOptimization GoldsteinPriceFunction
            => new AntColonyOptimization(2) { Objective = new GoldsteinPrice(2) };

        public static AntColonyOptimization RosenbrockFunction
            => new AntColonyOptimization(2) { Objective = new Rosenbrock(2) };

        public static AntColonyOptimization ZakharovFunction
            => new AntColonyOptimization(2) { Objective = new Zakharov(2) };

        private class SphereModel : ObjectiveFunction
        {
            public SphereModel(int dimension) : base(dimension, Objective.Minimize) {}

            protected override double EvaluateInternal(double[] steps) => Mozog.Examples.Functions.SphereModel(steps);
        }

        private class GoldsteinPrice : ObjectiveFunction
        {
            public GoldsteinPrice(int dimension) : base(dimension, Objective.Minimize) {}

            protected override double EvaluateInternal(double[] steps) => Mozog.Examples.Functions.GoldsteinPrice(steps);
        }

        private class Rosenbrock : ObjectiveFunction
        {
            public Rosenbrock(int dimension) : base(dimension, Objective.Minimize) {}

            protected override double EvaluateInternal(double[] steps) => Mozog.Examples.Functions.Rosenbrock(steps);
        }

        private class Zakharov : ObjectiveFunction
        {
            public Zakharov(int dimension) : base(dimension, Objective.Minimize) {}

            protected override double EvaluateInternal(double[] steps) => Mozog.Examples.Functions.Zakharov(steps);
        }
    }
}
