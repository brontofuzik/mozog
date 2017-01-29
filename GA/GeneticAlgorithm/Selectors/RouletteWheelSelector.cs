using System.Collections.Generic;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm.Selectors
{
    internal class RouletteWheelSelector<TGene> : ISelector<TGene>
    {
        private Population<TGene> population;
        private List<double> rouletteWheel;
        private double maxPocket;

        public void Initialize(Population<TGene> population)
        {
            this.population = population;

            rouletteWheel = new List<double>(population.Size);
            double currentPocket = 0.0;
            foreach (Chromosome<TGene> chromosome in population.Chromosomes)
            {
                rouletteWheel.Add(currentPocket += chromosome.Fitness);
            }
            maxPocket = currentPocket;
        }

        public Chromosome<TGene> Select()
        {
            double pocket = Random.Double(0.0, maxPocket);
            int index = rouletteWheel.BinarySearch(pocket);
            if (index < 0)
            {
                index = ~index;
            }
            return population[index]; 
        }
    }
}
