namespace AntColonyOptimization.Examples
{
    static class FunctionOptimization
    {
        public static AntColonyOptimization SphereModelFunction
            => new AntColonyOptimization(6) { Objective = ObjectiveFunction.Minimize(Mozog.Examples.Functions.SphereModel) };

        public static AntColonyOptimization GoldsteinPriceFunction
            => new AntColonyOptimization(2) { Objective = ObjectiveFunction.Minimize(Mozog.Examples.Functions.GoldsteinPrice) };

        public static AntColonyOptimization RosenbrockFunction
            => new AntColonyOptimization(2) { Objective = ObjectiveFunction.Minimize(Mozog.Examples.Functions.Rosenbrock) };

        public static AntColonyOptimization ZakharovFunction
            => new AntColonyOptimization(2) { Objective = ObjectiveFunction.Minimize(Mozog.Examples.Functions.Zakharov) };
    }
}
