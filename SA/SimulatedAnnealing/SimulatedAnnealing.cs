using System;

namespace SimulatedAnnealing
{
    public abstract class SimulatedAnnealing<T>
    {
        protected Random random = new Random();

        private ObjectiveFunction<T> objectiveFunction;

        private T[] bestState;
        private double bestEnergy = Double.MaxValue;

        public int Dimension => objectiveFunction.Dimension;

        public Objective Objective => objectiveFunction.Objective;

        public T[] Run(ObjectiveFunction<T> objectiveFunction,
            int maxIterations, out int usedIterations,
            double acceptableEnergy, out double achievedEnergy,
            double initialTemperature, double finalTemperature)
        {
            this.objectiveFunction = objectiveFunction;

            // s ← s0; e ← E(s) ... Iniital state, energy.
            T[] currentState = InitializeState();
            double currentEnergy = EvaluateState(currentState);

            // sbest ← s; ebest ← e ... Initial "best" solution
            bestState = currentState;
            bestEnergy = currentEnergy;

            // k ← 0 ... Energy evaluation count.
            int iterationIndex = 0;

            // while k < kmax and e > emax ... While time left & not good enough:
            while (iterationIndex < maxIterations && !IsDone(acceptableEnergy))
            {
                // snew ← neighbour(s) ... Pick some neighbour.
                T[] newState = PerturbState(currentState);

                // enew ← E(snew) ... Compute its energy.
                double newEnergy = EvaluateState(newState);

                // if enew < ebest then ... Is this a new best?
                if (newEnergy < bestEnergy)
                {
                    // sbest ← snew; ebest ← enew ... Save 'new neighbour' to 'best found'.
                    bestState = newState;
                    bestEnergy = newEnergy;
                }

                // if P(e, enew, temp(k/kmax)) > random() then ... Should we move to it?
                double temperature = CalculateTemperature(initialTemperature, finalTemperature, iterationIndex / (double)maxIterations);
                if (AcceptNewState(currentEnergy, newEnergy, temperature) > random.NextDouble())
                {
                    // s ← snew; e ← enew ... Yes, change state.
                    currentState = newState;
                    currentEnergy = newEnergy;
                }

                // k ← k + 1 ... One more evaluation done
                iterationIndex++;
            }

            // return sbest ... Return the best solution found.
            usedIterations = iterationIndex;
            achievedEnergy = objectiveFunction.Objective == Objective.Minimize ? bestEnergy : 1 / bestEnergy;
            return bestState;
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
        public T[] Run_Metropolis(double initialTemperature = 1000.0, double finalTemperature = 1.0, int kMax = 1000, double coolingCoefficient = 0.99, bool elitism = true)
        {
            // double T = T_max;
            double temperature = initialTemperature;

            // x_0 = náhodne vygenerovaný stav;
            T[] currentState = InitializeState();
            double currentEnergy = objectiveFunction.Evaluate(currentState);
            T[] bestState = currentState;
            double bestEnergy = currentEnergy;

            // while (T > T_min)
            while (temperature > finalTemperature)
            {
                // x_0 = Metropolis(x_0, kmax, T);
                currentState = Metropolis(currentState, kMax, temperature);
                currentEnergy = objectiveFunction.Evaluate(currentState);

                if (elitism && currentEnergy < bestEnergy)
                {
                    bestState = currentState;
                    bestEnergy = currentEnergy;
                }

                // T = a*T;
                temperature = coolingCoefficient * temperature;
            }

            // return x_0;
            return elitism ? bestState : currentState;
        }

        private T[] Metropolis(T[] initialState, int kMax, double temperature)
        {
            // x = x_0;
            T[] currentState = initialState;
            double currentEnergy = EvaluateState(currentState);

            // for (unsigned int k = 0 ; k < kmax ; k++)
            for (uint k = 0; k < kMax; k++)
            {
                // x' = O_pert(x);
                T[] newState = PerturbState(currentState);
                double newEnergy = EvaluateState(newState);

                // p = min(1, exp(-(f(x') - f(x)) / T));
                double acceptanceProbability = AcceptNewState(currentEnergy, newEnergy, temperature);

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

        protected abstract T[] InitializeState();

        protected abstract T[] PerturbState(T[] currentState);

        protected virtual double CalculateTemperature(double initialTemperature, double finalTemperature, double progress)
            => initialTemperature * Math.Pow(finalTemperature / initialTemperature, progress);

        protected virtual double AcceptNewState(double currentEnergy, double newEnergy, double temperature)
            => Math.Min(1, Math.Exp(-(newEnergy - currentEnergy) / temperature));

        private double EvaluateState(T[] state)
            => Objective == Objective.Minimize ? objectiveFunction.Evaluate(state) : 1 / objectiveFunction.Evaluate(state);

        public bool IsDone(double acceptableEnergy)
            => Objective == Objective.Minimize ? bestEnergy <= acceptableEnergy : 1 / bestEnergy >= acceptableEnergy;
    }
}
