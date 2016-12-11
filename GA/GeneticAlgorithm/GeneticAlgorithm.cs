using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    /// <remarks>
    /// A Genetic Algorithm Tutorial - The Canonical Genetic Algorithm (Darell Whitley)
    /// samizdat.mines.edu/ga_tutorial/ga_tutorial.ps
    /// </remarks>
    /// <typeparam name="TGene">The type of the gene.</typeparam>
    public abstract class GeneticAlgorithm< TGene >
    {
        #region Protected instance fields

        /// <summary>
        /// The psuedo-random number generator.
        /// </summary>
        protected Random random;

        #endregion // Protected instance fields

        #region Private instance fields

        /// <summary>
        /// The objective function.
        /// </summary>
        private ObjectiveFunction< TGene > objectiveFunction;

        /// <summary>
        /// The current population (pseudo-class).
        /// </summary>
        private List< Chromosome< TGene > > currentPopulation;

        /// <summary>
        /// The intermediate population (pseudo-class).
        /// </summary>
        private List< Chromosome< TGene > > intermediatePopulation;

        /// <summary>
        /// The next population (pseudo-class).
        /// </summary>
        private List< Chromosome< TGene > > nextPopulation;

        /// <summary>
        /// The global-best chromosome.
        /// </summary>
        private Chromosome< TGene > globalBestChromosome;

        /// <summary>
        /// Useful when the objective function is to be minimized.
        /// </summary>
        private double maxObjectiveFunctionValue;

        #endregion // Private instance fields

        #region Public instance properties

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

        #endregion // Public instance properties

        #region Protected instance constructors

        /// <summary>
        /// Creates a new genetic algorithm.
        /// </summary>
        protected GeneticAlgorithm()
        {
            random = new Random();
            objectiveFunction = null;
            currentPopulation = new List< Chromosome< TGene > >();
            intermediatePopulation = new List< Chromosome< TGene > >();
            nextPopulation = new List< Chromosome< TGene > >();
            globalBestChromosome = null;
        }

        #endregion // Protected instance constructors

        #region Public instance methods

        /// <summary>
        /// Runs the genetic algorithm.
        /// </summary>
        /// <param name="objectiveFunction">The objective function.</param>
        /// 
        /// <param name="maxGenerationCount">The maximum number of generations.</param>
        /// <param name="usedGenerationCount">The number of generation taken.</param>
        /// <param name="acceptableEvaluation">The acceptable evaluation (i.e. evaluation sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <param name="achievedEvalaution">The achieved evalaution.</param>
        ///
        /// <param name="populationSize">The size of the population (i.e. the number of chromosomes in the population).</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        /// <param name="scaling">The scaling flag.</param>
        /// <returns>
        /// The best solution (the global-best chromosome).
        /// </returns>
        public TGene[] Run( ObjectiveFunction< TGene > objectiveFunction,
            int maxGenerationCount, out int usedGenerationCount, double acceptableEvaluation, out double achievedEvalaution,
            int populationSize, double crossoverRate, double mutationRate, bool scaling
        )
        {
            if (objectiveFunction == null)
            {
                throw new ArgumentNullException( "objectiveFunction" );
            }
            this.objectiveFunction = objectiveFunction;

            // 1. The first step in the implementation of any genetic algorithm is to generate an initial population.
            // In most cases the initial population is generated randomly.
            CreateCurrentPopulation( populationSize );

            // Keep track of the global-best chromosome for elitist purposes.
            globalBestChromosome = currentPopulation[ 0 ];
            EvaluateChromosome( globalBestChromosome );

            // Repeat while the computational budget is not spend and an acceptable solution is not found.
            int generationIndex = 0;
            while ((generationIndex < maxGenerationCount) && !IsAcceptableSolutionFound( acceptableEvaluation ))
            {
                // 2. After creating an initial population, each string is then evaluated, ...
                EvaluateCurrentPopulation( scaling );

                // ... and assigned a fitness value (i.e. ranked).
                RankCurrentPopulation();

                // 3. We will first consider the construction of the intermediate population from the current population.
                // In the first generation the current population is also the the initial population.
                CreateIntermediatePopulation();

                // 4. After selection has been carried out the construction of the intermediate population is complete and recombination can occur.
                // This can be viewed as creating the next population from the intermediate population.
                CreateNextPopulation( crossoverRate, mutationRate );

                // 5. After process of selection, recombination and mutation is complete, the new population can be evaluated.
                currentPopulation = nextPopulation;

                // The process of evaluation, selection, recombination and mutation forms one generation.
                generationIndex++;
            }

            // Return the (global-best) solution.
            usedGenerationCount = generationIndex;
            achievedEvalaution = (objectiveFunction.Objective == Objective.MAXIMIZE) ? globalBestChromosome.Evaluation : ( maxObjectiveFunctionValue / globalBestChromosome.Evaluation);
            return globalBestChromosome.Genes;
        }

        #endregion // Public instance methods

        #region Protected instance methods

        /// <summary>
        /// <para>
        /// The generator function.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <returns>
        /// A random chromosome.
        /// </returns>
        protected abstract Chromosome< TGene > GeneratorFunction();

        /// <summary>
        /// <para>
        /// The fitness function.
        /// </para>
        /// <para>
        /// The <c>fitness function</c> transforms that measure (measure provided by the <i>evaluation function</i>)
        /// into an allocation of reproductiove opportunities.
        /// </para>
        /// <para>
        /// The fitness of that string, however, is always defined with respect to other members of the current population.
        /// </para>
        /// <para>
        /// In the caconical genetic algorithm, fitness is defined by: f_i / f_avg where f_i is the evaluation associated with string i
        /// and f_avg is the average evaluation of all the strings in the population. Fitness can be assigned based on a string's rank
        /// in the population or by sampling methods, such as tournament selection.
        /// </para>
        /// <para>
        /// Can be overriden.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to be ranked.</param>
        /// <param name="averageEvaluation">The average evaluation of all chromosomes in the (current) population.</param>
        protected virtual double FitnessFunction( Chromosome< TGene > chromosome, double averageEvaluation )
        {
            return (averageEvaluation != 0) ? (chromosome.Evaluation / averageEvaluation) : 1.0;
        }

        /// <summary>
        /// <para>
        /// The crossover function.
        /// </para>
        /// <para>
        /// Crossovers the chromosome with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <param name="parent1">The first parent to crossover.</param>
        /// <param name="parent2">The second parent to crossover.</param>
        /// <param name="offspring1">The first offpsring.</param>
        /// <param name="offspring2">The second offspring.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        protected abstract void CrossoverFunction( Chromosome< TGene > parent1, Chromosome< TGene > parent2, out Chromosome< TGene > offspring1, out Chromosome< TGene > offspring2, double crossoverRate );

        /// <summary>
        /// <para>
        /// The mutation function.
        /// </para>
        /// <para>
        /// Mutates the chromosome with some probability p_m.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to mutate.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        protected abstract void MutationFunction( Chromosome< TGene > chromosome, double mutationRate );

        #endregion // Protected instance methods

        #region Private instance methods

        /// <summary>
        /// Creates the initial population.
        /// </summary>
        /// <param name="populationSize">The size of the population (i.e. the number of chromosomes in the population).</param>
        private void CreateCurrentPopulation( int populationSize )
        {
            // Clear the current population, ...
            currentPopulation.Clear();

            // ... and populate it with new chromosomes.
            for (int i = 0; i < populationSize; i++)
            {
                // Generate a random chromosome, ...
                Chromosome< TGene > chromosome = GeneratorFunction();
                // ... and add it to the current generation.
                currentPopulation.Add( chromosome );
            }

            // Calculate the maximum objective function value.
            if (Objective == Objective.MINIMIZE)
            {
                maxObjectiveFunctionValue = Double.MinValue;
                foreach (Chromosome<TGene> chromosome in currentPopulation)
                {
                    double objectiveFunctionValue = objectiveFunction.Evaluate( chromosome.Genes );

                    if (objectiveFunctionValue > maxObjectiveFunctionValue)
                    {
                        maxObjectiveFunctionValue = objectiveFunctionValue;
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates the chromosomes in the current population.
        /// </summary>
        private void EvaluateCurrentPopulation( bool scaling )
        {
            // Keep track of the iteration-best chromosome  ...
            Chromosome< TGene > iterationBestChromosome = currentPopulation[ 0 ];
            // ... and the iteration-worst chromosome (for scaling purposes).
            Chromosome< TGene > iterationWorstChromosome = currentPopulation[ 0 ];

            foreach (Chromosome< TGene > chromosome in currentPopulation)
            {
                EvaluateChromosome( chromosome );

                // Update the iteration-best chromosome.
                if (chromosome.Evaluation > iterationBestChromosome.Evaluation)
                {
                    iterationBestChromosome = chromosome;
                }

                // Update the iteration-worst chromosome (for scaling purposes).
                if (scaling && chromosome.Evaluation < iterationWorstChromosome.Evaluation)
                {
                    iterationWorstChromosome = chromosome;
                }
            }

            // Update the global-best chromosome.
            if (iterationBestChromosome.Evaluation > globalBestChromosome.Evaluation)
            {
                globalBestChromosome = iterationBestChromosome;
            }

            // Scaling.
            // In the simplest case, one can subtract the evaluation of the worst string in the population from the evaluations
            // of all strings in the population. One can now compute the average string evaluation as well fitness values using
            // the adjusted evaluation, which will increase the resulting selective pressure.
            // Alternatively, one can use a rank based form of selection.
            if (scaling)
            {
                foreach (Chromosome< TGene > chromosome in currentPopulation)
                {
                    chromosome.Evaluation -= iterationWorstChromosome.Evaluation;
                }
            }
        }

        /// <summary>
        /// Assigns the fitnes values to the chromosomes in the current population.
        /// </summary>
        private void RankCurrentPopulation()
        {
            // Calculate the average evaluation.
            double totalEvaluation = 0.0;
            foreach (Chromosome< TGene > chromosome in currentPopulation)
            {
                totalEvaluation += chromosome.Evaluation;
            }
            double averageEvaluation = totalEvaluation / currentPopulation.Count;
            
            foreach (Chromosome< TGene > chromosome in currentPopulation)
            {
                chromosome.Fitness = FitnessFunction( chromosome, averageEvaluation );
            }
        }

        /// <summary>
        /// Creates the intermediate population from the current population.
        /// </summary>
        private void CreateIntermediatePopulation()
        {
            // Clear the intermediate population.
            intermediatePopulation.Clear();

            // 3.1. After calculating f_i / f_avg for all the strings in the current population, selection is carried out.
            // In the canonical genetic algorithm the probability that strings in the current population are copied (i.e., duplicated)
            // and placed in the intermediate generation is proportion to their fitness.

            // There are a number of ways to do selection. We might view the population as mapping onto a roulette wheel,
            // where each individual is represented by a space that proportionally corresponds to its fitness.
            List< double > rouletteWheel = new List< double >( currentPopulation.Count );
            double previousPocketSize = 0.0;
            foreach (Chromosome< TGene > chromosome in currentPopulation)
            {
                double currentPocketSize = previousPocketSize + chromosome.Fitness;
                rouletteWheel.Add( currentPocketSize );
                previousPocketSize = currentPocketSize;
            }

            // By repeatedly spinning the roulette wheel, individuals are chosen using "stochastic sampling with replacement"
            // to fill the intermediate population.
            for (int i = 0; i < currentPopulation.Count; i++)
            {
                double pocket = random.NextDouble() * currentPopulation.Count;
                int chromosomeIndex = rouletteWheel.BinarySearch( pocket );
                if (chromosomeIndex < 0)
                {
                    chromosomeIndex = ~chromosomeIndex;
                }
                Chromosome< TGene > chromosome = currentPopulation[ chromosomeIndex ];

                intermediatePopulation.Add( chromosome );
            }
        }

        /// <summary>
        /// Creates the next population from the intermediate population.
        /// </summary>
        private void CreateNextPopulation( double crossoverRate, double mutationRate )
        {
            // Clear the next population.
            nextPopulation.Clear();

            // 4.1. Crossover is applied to randomly paired strings with a probability denoted p_c.
            // (The population should already be sufficiently shuffled by the random selection process.)
            for (int i = 0; i < intermediatePopulation.Count; i += 2)
            {
                // Pick a pair of strings.
                Chromosome< TGene > parent1 = intermediatePopulation[ i ];
                Chromosome< TGene > parent2 = intermediatePopulation[ i + 1 ];

                // With probability p_c "recombine" these strings to form two new strings that are inserted into the next population.
                Chromosome< TGene > offspring1;
                Chromosome< TGene > offspring2;
                CrossoverFunction( parent1, parent2, out offspring1, out offspring2, crossoverRate );
                nextPopulation.Add( offspring1 );
                nextPopulation.Add( offspring2 );
            }

            // 4.2 After recombination, we can apply a mutation operator.
            // For each bit in the population, mutate with some low probability p_m.
            foreach (Chromosome< TGene > chromosome in nextPopulation)
            {
                MutationFunction( chromosome, mutationRate );
            }
        }

        /// <summary>
        /// Evaluates a chromosome.
        /// </summary>
        /// <param name="state">The chromosome to evaluate - the better the chromosome, the higher the evaluation.</param>
        /// <returns>
        /// The evaluation of the chromosome.
        /// </returns>
        public void EvaluateChromosome( Chromosome< TGene > chromosome )
        {
            chromosome.Evaluation = (Objective == Objective.MAXIMIZE) ?
                objectiveFunction.Evaluate( chromosome.Genes ) :
                (maxObjectiveFunctionValue / objectiveFunction.Evaluate( chromosome.Genes ));
        }

        /// <summary>
        /// Is acceptable solution found?
        /// </summary>
        /// <param name="acceptableEvaluation">The acceptable evaluation (i.e. evaluation sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <returns>
        /// <c>True</c> if an acceptable solution is found, <c>false</c> otherwise.
        /// </returns>
        public bool IsAcceptableSolutionFound( double acceptableEvaluation )
        {
            return (Objective == Objective.MAXIMIZE) ?
                (globalBestChromosome.Evaluation >= acceptableEvaluation) :
                ((maxObjectiveFunctionValue / globalBestChromosome.Evaluation) <= acceptableEvaluation);
        }

        #endregion // Private insatnce methods
    }
}
