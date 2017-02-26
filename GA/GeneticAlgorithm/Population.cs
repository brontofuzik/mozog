using System.Collections.Generic;
using Mozog.Utils;

namespace GeneticAlgorithm
{
    public class Population<TGene>
    {
        private readonly GeneticAlgorithm<TGene> algo;
        private readonly List<Chromosome<TGene>> chromosomes = new List<Chromosome<TGene>>();
        private Chromosome<TGene> bestChromosome;

        public static Population<TGene> CreateInitial(GeneticAlgorithm<TGene> algo, int size)
        {
            var population = new Population<TGene>(algo, 0);
            size.Times(() => population.chromosomes.Add(new Chromosome<TGene>(algo)));
            return population;
        }

        private Population(GeneticAlgorithm<TGene> algo, int generation)
        {
            this.algo = algo;
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
                chromosome.Evaluation = algo.Fitness.EvaluateObjective(chromosome);
                algo.Fitness.UpdateBestChromosome(chromosome, ref bestChromosome);
            }

            algo.Fitness.EvaluateFitness(this);

            return bestChromosome;
        }

        public Population<TGene> BreedNewGeneration()
        {
            var newPopulation = new Population<TGene>(algo, Generation + 1);

            algo.Selector.Initialize(this);

            (Size / 2).Times(() =>
            {
                Chromosome<TGene> parent1 = algo.Selector.Select();
                Chromosome<TGene> parent2 = algo.Selector.Select();
                var offsprings = parent1.Mate(parent2);
                offsprings.ForEach(o => newPopulation.chromosomes.Add(o));
            });

            return newPopulation;
        }
    }
}
