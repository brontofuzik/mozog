using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Mozog.Utils;

namespace GeneticAlgorithm
{
    public class Population<TGene>
    {
        private readonly List<Chromosome<TGene>> chromosomes = new List<Chromosome<TGene>>();
        private readonly GeneticAlgorithm<TGene> algo;

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
            // Evaluate objective function.
            algo.Parallelizer.Parallelize(chromosomes.Select<Chromosome<TGene>, Action>(c =>
            {
                var cNew = c;
                return () => { cNew.Evaluation = algo.Fitness.EvaluateObjective(cNew); };
            }));

            // Evaluate fitness function.
            algo.Fitness.EvaluateFitness(this);

            return chromosomes.MaxBy(c => c.Fitness);
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
                Extensions.ForEach(offsprings, o => newPopulation.chromosomes.Add(o));
            });

            return newPopulation;
        }
    }
}
