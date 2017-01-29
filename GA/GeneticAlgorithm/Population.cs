using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace GeneticAlgorithm
{
    public class Population<TGene>
    {
        private readonly Parameters<TGene> args;
        private readonly List<Chromosome<TGene>> chromosomes = new List<Chromosome<TGene>>();
        private Chromosome<TGene> bestChromosome;
        private Chromosome<TGene> worstChromosome;

        public static Population<TGene> CreateInitial(Parameters<TGene> args)
        {
            var population = new Population<TGene>(args, 0);
            args.PopulationSize.Times(() => population.chromosomes.Add(new Chromosome<TGene>(args)));
            return population;
        }

        private Population(Parameters<TGene> args, int generation)
        {
            this.args = args;
            Generation = generation;
        }

        public int Generation { get; }

        public IEnumerable<Chromosome<TGene>> Chromosomes => chromosomes;

        public Chromosome<TGene> this[int index] => chromosomes[index];

        public int Size => chromosomes.Count;

        public Chromosome<TGene> EvaluateFitness()
        {
            // Evaluate
            foreach (Chromosome<TGene> chromosome in chromosomes)
            {
                chromosome.Evaluation = args.ObjectiveFunction.Evaluate(chromosome);
                args.ObjectiveFunction.UpdateBestChromosome(ref bestChromosome, chromosome);

                if (args.Scaling)
                {
                    args.ObjectiveFunction.UpdateWorstChromosome(ref worstChromosome, chromosome);
                }
            }

            if (args.Scaling)
            {
                Scale();
            }

            // Assign fitness
            double averageEvaluation = chromosomes.Average(c => c.Evaluation);
            chromosomes.ForEach(c => c.Fitness = args.FitnessFunction(c.Evaluation, averageEvaluation, args.ObjectiveFunction.Objective));

            return bestChromosome;
        }

        public Population<TGene> BreedNewGeneration()
        {
            var newPopulation = new Population<TGene>(args, Generation + 1);

            args.Selector.Initialize(this);

            (Size / 2).Times(() =>
            {
                Chromosome<TGene> parent1 = args.Selector.Select();
                Chromosome<TGene> parent2 = args.Selector.Select();
                var offsprings = parent1.Mate(parent2);
                offsprings.ForEach(o => newPopulation.chromosomes.Add(o));
            });

            return newPopulation;
        }

        // Scaling.
        // In the simplest case, one can subtract the evaluation of the worst string in the population from the evaluations
        // of all strings in the population. One can now compute the average string evaluation as well fitness values using
        // the adjusted evaluation, which will increase the resulting selective pressure.
        // Alternatively, one can use a rank based form of selection.
        private void Scale()
        {
            chromosomes.ForEach(c => c.Evaluation -= worstChromosome.Evaluation);
        }
    }
}
