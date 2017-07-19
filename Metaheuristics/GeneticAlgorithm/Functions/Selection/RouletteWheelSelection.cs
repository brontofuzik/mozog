﻿using System.Collections.Generic;
using Mozog.Utils.Math;

namespace GeneticAlgorithm.Functions.Selection
{
    internal class RouletteWheelSelection<TGene> : SelectionFunction<TGene>
    {     
        private List<double> rouletteWheel;
        private double maxPocket;

        public override void Initialize(Population<TGene> population)
        {
            Population = population;

            rouletteWheel = new List<double>(population.Size);
            double currentPocket = 0.0;
            foreach (Chromosome<TGene> chromosome in population.Chromosomes)
            {
                rouletteWheel.Add(currentPocket += chromosome.Fitness);
            }
            maxPocket = currentPocket;
        }

        public override Chromosome<TGene> Select()
        {
            double pocket = StaticRandom.Double(0.0, maxPocket);
            int index = rouletteWheel.BinarySearch(pocket);
            if (index < 0)
            {
                index = ~index;
            }
            return Population[index]; 
        }
    }
}
