using System;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;

namespace SimulatedAnnealing
{
    public class SimulatedAnnealing<T>
    {     
        // Stopping criteria
        private int maxIterations;
        private double targetEnergy;

        public SimulatedAnnealing(int dimension)
        {
            Dimension = dimension;
        }

        public int Dimension { get; }

        #region Functions

        private ObjectiveFunction<T> objective;
        public ObjectiveFunction<T> Objective
        {
            get { return objective; }
            set
            {
                objective = value;
                objective.Algo = this;
            }
        }

        private IInitializationFunction<T> initializer;
        public IInitializationFunction<T> Initializer
        {
            get { return initializer; }
            set
            {
                initializer = value;
                initializer.Algo = this;
            }
        }

        private IPerturbationFunction<T> perturbator;
        public IPerturbationFunction<T> Perturbator
        {
            get { return perturbator; }
            set
            {
                perturbator = value;
                initializer.Algo = this;
            }
        }

        #endregion // Functions

        public Result<T> Run(int maxIterations, double initialTemperature, double finalTemperature, double targetEnergy = Double.MinValue)
        {
            this.maxIterations = maxIterations;
            this.targetEnergy = targetEnergy;

            var currentState = new State<T>(this);

            int iteration = 0;
            while (!IsDone(currentState.E, iteration))
            {
                var newState = currentState.Perturb();

                double temperature = CalculateTemperature(initialTemperature, finalTemperature, iteration / (double)maxIterations);
                Probability acceptProbability = AcceptNewState(currentState.E, newState.E, temperature);
                if (acceptProbability)
                {
                    currentState = newState;
                }

                iteration++;
            }

            return new Result<T>(currentState, iteration);
        }

        /*
        public State<T> Run_Metropolis(double initialTemperature = 1000.0, double finalTemperature = 1.0, double coolingCoefficient = 0.99, int kMax = 1000)
        {
            var currentState = new State<T>(this);

            double temperature = initialTemperature;
            while (temperature > finalTemperature)
            {
                currentState = Metropolis(currentState, kMax, temperature);

                temperature *= coolingCoefficient;
            }

            return currentState;
        }

        private State<T> Metropolis(State<T> currentState, int kMax, double temperature)
        {
            for (int k = 0; k < kMax; k++)
            {
                var newState = currentState.Perturb();

                Probability acceptanceProbability = AcceptNewState(currentState.E, newState.E, temperature);
                if (acceptanceProbability)
                {
                    currentState = newState;
                }
            }

            return currentState;
        }
        */

        protected virtual double CalculateTemperature(double initialTemperature, double finalTemperature, double progress)
            => initialTemperature * System.Math.Pow(finalTemperature / initialTemperature, progress);

        protected virtual double AcceptNewState(double currentEnergy, double newEnergy, double temperature)
            => System.Math.Min(1, System.Math.Exp(-(newEnergy - currentEnergy) / temperature));

        private bool IsDone(double energy, int iteration) => energy <= targetEnergy || iteration >= maxIterations;
    }

    public struct Result<T>
    {
        public State<T> State { get; }

        public int Iterations { get; }

        public Result(State<T> state, int iterations)
        {
            State = state;
            Iterations = iterations;
        }
    }
}