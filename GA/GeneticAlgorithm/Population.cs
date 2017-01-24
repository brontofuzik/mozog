using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace GeneticAlgorithm
{
    public class Population<TGene>
    {
        private readonly List<Chromosome<TGene>> chromosomes = new List<Chromosome<TGene>>();

        private Chromosome<TGene> bestChromosome;
        private Chromosome<TGene> worstChromosome;

        public Population(int generation)
        {
            Generation = generation;
        }

        public static Population<TGene> CreateRandom(int size, InitializationFunction<TGene> initializer)
        {
            var population = new Population<TGene>(0);
            size.Times(() => population.chromosomes.Add(initializer()));
            return population;
        }

        public int Generation { get; }

        public IEnumerable<Chromosome<TGene>> Chromosomes => chromosomes;

        public Chromosome<TGene> this[int index] => chromosomes[index];

        public int Size => chromosomes.Count;

        public double Evaluate(ObjectiveFunction<TGene> objectiveFunction, Objective objective)
        {
            var evaluations = chromosomes.Select(c => c.Evaluate(objectiveFunction));
            return objective == Objective.Maximize ? evaluations.Max() : evaluations.Min();
        }

        public Chromosome<TGene> Evaluate(ObjectiveFunction<TGene> objectiveFunction, Objective objective, bool scaling)
        {
            foreach (Chromosome<TGene> chromosome in chromosomes)
            {
                chromosome.Evaluate(objectiveFunction);

                objectiveFunction.UpdateBestChromosome(ref bestChromosome, chromosome);

                if (scaling)
                {
                    objectiveFunction.UpdateWorstChromosome(ref worstChromosome, chromosome);
                }
            }

            if (scaling)
            {
                Scale();
            }

            return bestChromosome;
        }

        public void EvaluateFitness(FitnessFunction fitnessFunction, Objective objective)
        {
            double averageEvaluation = chromosomes.Average(c => c.Evaluation);
            chromosomes.ForEach(c => c.EvaluateFitness(fitnessFunction, averageEvaluation, objective));
        }

        // Scaling.
        // In the simplest case, one can subtract the evaluation of the worst string in the population from the evaluations
        // of all strings in the population. One can now compute the average string evaluation as well fitness values using
        // the adjusted evaluation, which will increase the resulting selective pressure.
        // Alternatively, one can use a rank based form of selection.
        private void Scale()
        {
            foreach (Chromosome<TGene> chromosome in chromosomes)
            {
                chromosome.Evaluation -= worstChromosome.Evaluation;
            }
        }

        public Population<TGene> BreedNewGeneration(ISelector<TGene> selector, CrossoverFunction<TGene> crossover, double crossoverRate,
            MutationFunction<TGene> mutator, double mutationRate)
        {
            var newPopulation = new Population<TGene>(Generation + 1);

            selector.Initialize(this);

            (Size / 2).Times(() =>
            {
                // Parents
                Chromosome<TGene> parent1 = selector.Select();
                Chromosome<TGene> parent2 = selector.Select();

                // Offsprings
                Chromosome<TGene> offspring1;
                Chromosome<TGene> offspring2;
                crossover(parent1, parent2, out offspring1, out offspring2, crossoverRate);

                mutator(offspring1, mutationRate);
                mutator(offspring2, mutationRate);

                newPopulation.chromosomes.Add(offspring1);
                newPopulation.chromosomes.Add(offspring2);
            });

            return newPopulation;
        }
    }
}
