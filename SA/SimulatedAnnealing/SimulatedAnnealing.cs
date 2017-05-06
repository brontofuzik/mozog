using Mozog.Utils.Math;

namespace SimulatedAnnealing
{
    public abstract class SimulatedAnnealing<T>
    {
        private ObjectiveFunction<T> objectiveFunction;

        // Stopping criteria
        private int maxIterations;
        private double targetEnergy;

        public int Dimension => objectiveFunction.Dimension;

        public Objective Objective => objectiveFunction.Objective;

        public Result<T> Run(ObjectiveFunction<T> objectiveFunction,
            int maxIterations, double targetEnergy, double initialTemperature, double finalTemperature)
        {
            this.objectiveFunction = objectiveFunction;
            this.maxIterations = maxIterations;
            this.targetEnergy = targetEnergy;

            var currentState = CreateState(InitializeState());

            int iteration = 0;
            while (!IsDone(currentState.Energy, iteration))
            {
                var newState = CreateState(PerturbState(currentState.S));

                double temperature = CalculateTemperature(initialTemperature, finalTemperature, iteration / (double)maxIterations);
                Probability acceptProbability = AcceptNewState(currentState.Energy, newState.Energy, temperature);
                if (acceptProbability)
                {
                    currentState = newState;
                }

                iteration++;
            }

            return new Result<T>(currentState, iteration);
        }

        //public State<T> Run_Metropolis(double initialTemperature = 1000.0, double finalTemperature = 1.0, int kMax = 1000, double coolingCoefficient = 0.99)
        //{
        //    var currentState = CreateState(InitializeState());

        //    double temperature = initialTemperature;
        //    while (temperature > finalTemperature)
        //    {
        //        currentState = Metropolis(currentState, kMax, temperature);

        //        temperature *= coolingCoefficient;
        //    }

        //    return currentState;
        //}

        //private State<T> Metropolis(State<T> initialState, int kMax, double temperature)
        //{
        //    var currentState = initialState;

        //    for (uint k = 0; k < kMax; k++)
        //    {
        //        var newState = CreateState(PerturbState(currentState.S));

        //        Probability acceptanceProbability = AcceptNewState(currentState.Energy, newState.Energy, temperature);
        //        if (acceptanceProbability)
        //        {
        //            currentState = newState;
        //        }
        //    }

        //    return currentState;
        //}

        protected abstract T[] InitializeState();

        protected abstract T[] PerturbState(T[] currentState);

        protected virtual double CalculateTemperature(double initialTemperature, double finalTemperature, double progress)
            => initialTemperature * System.Math.Pow(finalTemperature / initialTemperature, progress);

        protected virtual double AcceptNewState(double currentEnergy, double newEnergy, double temperature)
            => System.Math.Min(1, System.Math.Exp(-(newEnergy - currentEnergy) / temperature));

        private bool IsDone(double energy, int iteration) => energy <= targetEnergy || iteration >= maxIterations;

        private State<T> CreateState(T[] s) => new State<T>(s, objectiveFunction.Evaluate(s));
    }

    public struct State<T>
    {
        public T[] S { get; }

        public double Energy { get; }

        public State(T[] state, double energy)
        {
            S = state;
            Energy = energy;
        }
    }

    public struct Result<T>
    {
        private State<T> State { get; }

        private int Iterations { get; }

        public Result(State<T> state, int iterations)
        {
            State = state;
            Iterations = iterations;
        }
    }
}