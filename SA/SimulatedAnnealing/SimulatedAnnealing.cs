using System;

namespace SimulatedAnnealing
{
    public abstract class SimulatedAnnealing<T>
    {
        private ObjectiveFunction<T> objectiveFunction;

        // Stopping criteria
        private int maxIterations;
        private double targetEnergy;

        //public int Dimension => objectiveFunction.Dimension;

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
                Probability acceptanceProbability = AcceptNewState(currentState.Energy, newState.Energy, temperature);
                if (acceptanceProbability)
                {
                    currentState = newState;
                }

                iteration++;
            }

            return new Result<T>(currentState, iteration);
        }

        /// <summary>
        /// <para>
        /// Runs the simulated annealing using the Metropolis algorithm.
        /// </para>
        /// <para>
        /// Simulované žíhání (Simulated Annealing), Metropolisuv algoritmus (Zdeněk VAŠÍČEK)
        /// http://www.stud.fit.vutbr.cz/~xvasic11/projects/msi.pdf
        /// </para>
        /// </summary>
        /// <param name="initialTemperature">The initial temperature (T_max).</param>
        /// <param name="finalTemperature">The final temperature (T_min).</param>
        /// <param name="kMax">The maximum number of k.</param>
        /// <param name="coolingCoefficient">The cooling coefficient.</param>
        /// <param name="elitism">The elitism flag.</param>
        /// <returns>
        /// The best solution (i.e. the global-best state).
        /// </returns>
        public State<T> Run_Metropolis(double initialTemperature = 1000.0, double finalTemperature = 1.0, int kMax = 1000, double coolingCoefficient = 0.99, bool elitism = true)
        {
            var currentState = CreateState(InitializeState());
            var bestState = currentState;

            double temperature = initialTemperature;
            while (temperature > finalTemperature)
            {
                currentState = Metropolis(currentState, kMax, temperature);

                if (elitism && currentState.Energy < bestState.Energy)
                {
                    bestState = currentState;
                }

                temperature *= coolingCoefficient;
            }

            return elitism ? bestState : currentState;
        }

        private State<T> Metropolis(State<T> initialState, int kMax, double temperature)
        {
            var currentState = initialState;

            for (uint k = 0; k < kMax; k++)
            {
                var newState = CreateState(PerturbState(currentState.S));

                Probability acceptanceProbability = AcceptNewState(currentState.Energy, newState.Energy, temperature);
                if (acceptanceProbability)
                {
                    currentState = newState;
                }
            }

            return currentState;
        }

        protected abstract T[] InitializeState();

        protected abstract T[] PerturbState(T[] currentState);

        protected virtual double CalculateTemperature(double initialTemperature, double finalTemperature, double progress)
            => initialTemperature * Math.Pow(finalTemperature / initialTemperature, progress);

        protected virtual double AcceptNewState(double currentEnergy, double newEnergy, double temperature)
            => Math.Min(1, Math.Exp(-(newEnergy - currentEnergy) / temperature));

        private bool IsDone(double energy, int iteration) => energy <= targetEnergy || iteration >= maxIterations;

        private State<T> CreateState(T[] s) => new State<T>(s, objectiveFunction.Evaluate(s));
    }

    public struct Probability
    {
        private static readonly Random r = new Random();
        private readonly double p;

        public Probability(double p)
        {
            this.p = p;
        }

        public static implicit operator Probability(double p)
            => new Probability(p);

        public static implicit operator Boolean(Probability probability)
            => r.NextDouble() < probability.p;
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
