using System;
using System.Linq;
using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Fitness
{
    public class FitnessFunction<TGene> : FunctionBase<TGene>, IFitnessFunction<TGene>
    {
        public static FitnessFunction<TGene> Maximize(Func<TGene[], double> objectiveFunction)
            => new FitnessFunction<TGene>(objectiveFunction, Objective.Maximize);

        public static FitnessFunction<TGene> Minimize(Func<TGene[], double> objectiveFunction)
            => new FitnessFunction<TGene>(objectiveFunction, Objective.Minimize);

        private FitnessFunction(Func<TGene[], double> objectiveFunction, Objective objective)
        {
            Objective = objective;
            ObjectiveFunction = objectiveFunction;
        }

        public Func<TGene[], double> ObjectiveFunction { get; }

        public Objective Objective { get; }

        private bool Maximizing => Objective == Objective.Maximize;

        private bool Minimizing => Objective == Objective.Minimize;

        public double EvaluateObjective(Chromosome<TGene> chromosome) => ObjectiveFunction(chromosome.Genes);

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
        public void EvaluateFitness(Population<TGene> population)
        {
            double averageEvaluation = population.Chromosomes.Average(c => c.Evaluation);
            population.Chromosomes.ForEach(c => c.Fitness = Maximizing ? c.Evaluation / averageEvaluation : averageEvaluation / c.Evaluation);
        }

        public bool IsAcceptable(double evaluation, double acceptableEvaluation)
            => Maximizing ? evaluation >= acceptableEvaluation : evaluation <= acceptableEvaluation;
    }

    /// <summary>
    /// The objective - minimize or maximize the objective function?
    /// </summary>
    public enum Objective
    {
        Minimize,
        Maximize
    }
}
