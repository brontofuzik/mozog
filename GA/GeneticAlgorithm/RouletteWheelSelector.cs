using System.Collections.Generic;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm
{
    internal class RouletteWheelSelector<TGene> : ISelector<TGene>
    {
        private Population<TGene> population;
        private List<double> rouletteWheel;

        public void Initialize(Population<TGene> population)
        {
            rouletteWheel = new List<double>(population.Size);
            double previousPocketSize = 0.0;
            foreach (Chromosome<TGene> chromosome in population.Chromosomes)
            {
                double currentPocketSize = previousPocketSize + chromosome.Fitness;
                rouletteWheel.Add(currentPocketSize);
                previousPocketSize = currentPocketSize;
            }
        }

        public Chromosome<TGene> Select()
        {
            double pocket = Random.Double(0.0, population.Size);
            int index = rouletteWheel.BinarySearch(pocket);
            if (index < 0)
            {
                index = ~index;
            }
            return population[index]; 
        }
    }
}
