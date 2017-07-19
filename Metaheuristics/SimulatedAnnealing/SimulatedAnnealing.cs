using System;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Cooling;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;
using static System.Math;

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
            get => objective;
            set
            {
                objective = value;
                objective.Algo = this;
            }
        }

        private IInitializationFunction<T> initialization;
        public IInitializationFunction<T> Initialization
        {
            get => initialization;
            set
            {
                initialization = value;
                initialization.Algo = this;
            }
        }

        private IPerturbationFunction<T> perturbation;
        public IPerturbationFunction<T> Perturbation
        {
            get => perturbation;
            set
            {
                perturbation = value;
                initialization.Algo = this;
            }
        }

        public ICoolingFunction Cooling { get; set; }

        #endregion // Functions

        public Result<T> Run(double initialTemperature, double finalTemperature = 0.0, int maxIterations = Int32.MaxValue, double targetEnergy = Double.MinValue)
        {
            this.maxIterations = maxIterations;
            this.targetEnergy = targetEnergy;
            Cooling.SetParams(initialTemperature, finalTemperature, maxIterations);

            var currentState = new State<T>(this);

            int iteration = 0;
            while (!Terminate(iteration, currentState.E))
            {
                var candidateState = currentState.Perturb();

                double temperature = Cooling.CoolTemperature(iteration);

                if (AcceptCandidateState(candidateState, currentState, temperature))
                {
                    currentState = candidateState;
                }

                iteration++;
            }

            return new Result<T>(currentState, iteration);
        }

        // Metropolis-Hastings criterion
        private Probability AcceptCandidateState(State<T> candidateState, State<T> currentState, double temperature)
            => new Probability(Min(1, Exp(-(candidateState.E - currentState.E) / temperature)));

        private bool Terminate(int iteration, double energy)
            => iteration >= maxIterations || energy <= targetEnergy ;
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