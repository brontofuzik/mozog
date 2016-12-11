using System;

namespace SimulatedAnnealing
{
    /// <remarks>
    /// Simulated annealing
    /// http://en.wikipedia.org/wiki/Simulated_annealing
    /// </remarks>
    /// <typeparam name="TAtom">The type of the atom.</typeparam>
    public abstract class SimulatedAnnealing< T >
    {
        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        protected Random random;

        /// <summary>
        /// The objective function.
        /// </summary>
        private ObjectiveFunction< T > objectiveFunction;

        /// <summary>
        /// The best state.
        /// </summary>
        private T[] bestState;

        /// <summary>
        /// The best energy.
        /// </summary>
        private double bestEnergy;

        /// <summary>
        /// Creates a new simulated annealing.
        /// </summary>
        protected SimulatedAnnealing()
        {
            random = new Random();
            objectiveFunction = null;
            bestState = null;
            bestEnergy = Double.MaxValue;
        }

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public int Dimension
        {
            get
            {
                return objectiveFunction.Dimension;
            }
        }

        /// <summary>
        /// Gets the objective (minimize or maximize).
        /// </summary>
        /// <value>
        /// The objective (minimize or mazimize).
        /// </value>
        public Objective Objective
        {
            get
            {
                return objectiveFunction.Objective;
            }
        }

        /// <summary>
        /// Runs the simulated annealing.
        /// </summary>
        /// <param name="objectiveFunction">The objective function.</param>
        /// 
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="usedIterationCount">The number of used iterations.</param>
        /// <param name="acceptableEnergy">The acceptable energy (i.e. energy sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <param name="achieevdEnergy">The achieved energy.</param>
        /// 
        /// <param name="initialTemperature">The initial temperature.</param>
        /// <param name="finalTemperature">The final temperature.</param>
        /// <returns>
        /// The best solution (i.e. the global-best state).
        /// </returns>
        public T[] Run( ObjectiveFunction< T > objectiveFunction,
            int maxIterationCount, out int usedIterationCount,
            double acceptableEnergy, out double achievedEnergy,
            double initialTemperature, double finalTemperature
        )
        {
            if (objectiveFunction == null)
            {
                throw new ArgumentNullException( "objectiveFunction" );
            }
            this.objectiveFunction = objectiveFunction;

            // s ← s0; e ← E(s) ... Iniital state, energy.
            T[] currentState = GeneratorFunction();
            double currentEnergy = EvaluateState( currentState );

            // sbest ← s; ebest ← e ... Initial "best" solution
            bestState = currentState;
            bestEnergy = currentEnergy;

            // k ← 0 ... Energy evaluation count.
            int iterationIndex = 0;

            // while k < kmax and e > emax ... While time left & not good enough:
            while ((iterationIndex < maxIterationCount) && !IsAcceptableSolutionFound( acceptableEnergy ))
            {
                // snew ← neighbour(s) ... Pick some neighbour.
                T[] newState = PerturbationFunction( currentState );

                // enew ← E(snew) ... Compute its energy.
                double newEnergy = EvaluateState( newState );

                // if enew < ebest then ... Is this a new best?
                if (newEnergy < bestEnergy)
                {
                    // sbest ← snew; ebest ← enew ... Save 'new neighbour' to 'best found'.
                    bestState = newState;
                    bestEnergy = newEnergy;
                }

                // if P(e, enew, temp(k/kmax)) > random() then ... Should we move to it?
                double temperature = TemperatureFunction( initialTemperature, finalTemperature, (iterationIndex / (double)maxIterationCount) );
                if (AcceptanceProbabilityFunction( currentEnergy, newEnergy, temperature ) > random.NextDouble())
                {
                    // s ← snew; e ← enew ... Yes, change state.
                    currentState = newState;
                    currentEnergy = newEnergy;
                }

                // k ← k + 1 ... One more evaluation done
                iterationIndex++;
            }

            // return sbest ... Return the best solution found.
            usedIterationCount = iterationIndex;
            achievedEnergy = (objectiveFunction.Objective == Objective.MINIMIZE) ? bestEnergy : (1 / bestEnergy);
            return bestState;
        }

        ///// <summary>
        ///// <para>
        ///// Runs the simulated annealing using the Metropolis algorithm.
        ///// </para>
        ///// <para>
        ///// Simulované žíhání (Simulated Annealing), Metropolisuv algoritmus (Zdeněk VAŠÍČEK)
        ///// http://www.stud.fit.vutbr.cz/~xvasic11/projects/msi.pdf
        ///// </para>
        ///// </summary>
        ///// <param name="initialTemperature">The initial temperature (T_max).</param>
        ///// <param name="finalTemperature">The final temperature (T_min).</param>
        ///// <param name="kMax">The maximum number of k.</param>
        ///// <param name="coolingCoefficient">The cooling coefficient.</param>
        ///// <param name="elitism">The elitism flag.</param>
        ///// <returns>
        ///// The best solution (i.e. the global-best state).
        ///// </returns>
        //public T[] RunUsingMetropolisAlgorithm( double initialTemperature, double finalTemperature, int kMax, double coolingCoefficient, bool elitism )
        //{
        //    // double T = T_max;
        //    double temperature = initialTemperature;

        //    // x_0 = náhodne vygenerovaný stav;
        //    T[] currentState = GeneratorFunction();
        //    double currentEnergy = ObjectiveFunction( currentState );
        //    T[] bestState = currentState;
        //    double bestEnergy = currentEnergy;

        //    // while (T > T_min)
        //    while (temperature > finalTemperature)
        //    {
        //        // x_0 = Metropolis(x_0, kmax, T);
        //        currentState = MetropolisAlgorithm( currentState, kMax, temperature );
        //        currentEnergy = ObjectiveFunction( currentState );
                
        //        if (elitism && currentEnergy < bestEnergy)
        //        {
        //            bestState = currentState;
        //            bestEnergy = currentEnergy;
        //        }

        //        // T = a*T;
        //        temperature = coolingCoefficient * temperature;
        //    }

        //    // return x_0;
        //    return elitism ? bestState : currentState;
        //}

        ///// <summary>
        ///// Runs the simulated annealing using the Metropolis algoritm.
        ///// </summary>
        ///// <returns>
        ///// The best solution (i.e. the global-best state).
        ///// </returns>
        //public T[] RunUsingMetropolisAlgorithm()
        //{
        //    return RunUsingMetropolisAlgorithm( 1000.0, 1.0, 1000, 0.99, true );
        //}

        ///// <summary>
        ///// Runs the simulated annealing using the Metropolis algorithm "ruby-way".
        ///// </summary>
        ///// <param name="args">The arguments.</param>
        ///// <returns>
        ///// The best solution (i.e. the global-best state).
        ///// </returns>
        //public T[] RunUsingMetropolisAlgorithm( Dictionary< string, object > args )
        //{
        //    // Validate the presence of optional parameters.

        //    // The initial temperature.
        //    double initialTemperature = (args.ContainsKey( "initialTemperature" )) ? (double)args[ "initialTemperature" ] : 1000.0;
            
        //    // The final temperature.
        //    double finalTemperature = (args.ContainsKey( "finalTemperature" )) ? (double)args[ "finalTemperature" ] : 1.0;
            
        //    // The maximum number of k.
        //    int kMax = (args.ContainsKey( "kMax" )) ? (int)args[ "kMax" ] : 1000;

        //    // The cooling coefficient.
        //    double coolingCoefficient = (args.ContainsKey( "coolingCoefficient" )) ? (double)args[ "coolingCoefficient" ] : 0.99;
            
        //    // The elitism flag.
        //    bool elitism  = (args.ContainsKey( "elitism" )) ? (bool)args[ "elitism" ] : true;

        //    return RunUsingMetropolisAlgorithm( initialTemperature, finalTemperature, kMax, coolingCoefficient, elitism );
        //}

        /// <summary>
        /// <para>
        /// The generator function.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <returns>
        /// A random state of the system.
        /// </returns>
        protected abstract T[] GeneratorFunction();

        /// <summary>
        /// <para>
        /// The perturbation (a.k.a. neighbour) function - generates a randomly chosen neighbour of a given state.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <param name="currentState">The current state of the system.</param>
        /// <returns>
        /// The new state of the system.
        /// </returns>
        protected abstract T[] PerturbationFunction( T[] currentState );

        /// <summary>
        /// The temperature funnction - defines the annealing schedule.
        /// </summary>
        /// <param name="initialTemperature">The initial temperature.</param>
        /// <param name="r">The fraction of the time budget that has been expended so far.</param>
        /// <returns>
        /// The temperature the use.
        /// </returns>
        protected virtual double TemperatureFunction( double initialTemperature, double finalTemperature, double r )
        {
            return initialTemperature * Math.Pow( (finalTemperature / initialTemperature), r );
        }

        /// <summary>
        /// <para>
        /// The acceptance probability function - specifies the probability of making the transition from the current state s to a candidate new state s'.
        /// </para>
        /// <para>
        /// Can be overriden.
        /// </para>
        /// </summary>
        /// <param name="currentEnergy">The current (internal) energy of the system.</param>
        /// <param name="newEnergy">The new (internal) energy of the system.</param>
        /// <param name="temperature">The temperature of the system.</param>
        /// <returns>
        /// The probability that a transition from the current state of the system to the new state of the system will be accepted.
        /// </returns>
        protected virtual double AcceptanceProbabilityFunction( double currentEnergy, double newEnergy, double temperature )
        {
            return Math.Min( 1, Math.Exp( - (newEnergy - currentEnergy) / temperature ) );
        }

        /// <summary>
        /// Runs the Metropolis algorithm.
        /// </summary>
        /// <param name="initialState">The initial state of the system.</param>
        /// <param name="kMax">The maximum number of k.</param>
        /// <param name="temperature">The temperature of the system.</param>
        /// <returns>
        /// The final state of the system.
        /// </returns>
        private T[] MetropolisAlgorithm( T[] initialState, int kMax, double temperature )
        {
            // x = x_0;
            T[] currentState = initialState;
            double currentEnergy = EvaluateState( currentState );

            // for (unsigned int k = 0 ; k < kmax ; k++)
            for (uint k = 0; k < kMax; k++)
            {
                // x' = O_pert(x);
                T[] newState = PerturbationFunction( currentState );
                double newEnergy = EvaluateState( newState );

                // p = min(1, exp(-( f(x') - f(x) ) / T));
                double acceptanceProbability = AcceptanceProbabilityFunction( currentEnergy, newEnergy, temperature );

                // if (uniform(0, 1) < p)
                if (random.NextDouble() < acceptanceProbability)
                {
                    // x = x';
                    currentState = newState;
                    currentEnergy = newEnergy;
                }
            }

            // return x;
            return currentState;
        }

        /// <summary>
        /// Evaluates a state - the better the state, the lower the evaluation.
        /// </summary>
        /// <param name="state">The state to evalaute.</param>
        /// <returns>
        /// The evaluation of the state.
        /// </returns>
        private double EvaluateState( T[] state )
        {
            return (Objective == Objective.MINIMIZE) ? objectiveFunction.Evaluate( state ) : (1 / objectiveFunction.Evaluate( state ));
        }

        /// <summary>
        /// Is acceptable solution found?
        /// </summary>
        /// <param name="acceptableEnergy">The acceptable energy (i.e. energy sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <returns>
        /// <c>True</c> if an acceptable solution is found, <c>false</c> otherwise.
        /// </returns>
        public bool IsAcceptableSolutionFound( double acceptableEnergy )
        {
            return (Objective == Objective.MINIMIZE) ? (bestEnergy <= acceptableEnergy) : ((1 / bestEnergy) >= acceptableEnergy);
        }
    }
}
