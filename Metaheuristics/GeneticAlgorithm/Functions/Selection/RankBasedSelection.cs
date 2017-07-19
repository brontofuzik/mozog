using System.Linq;
using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Selection
{
    public class RankBasedSelection<TGene> : SelectionFunction<TGene>
    {
        private readonly RouletteWheelSelection<TGene> rouletteWheel = new RouletteWheelSelection<TGene>();

        public override void Initialize(Population<TGene> population)
        {
            ReevaluateFitness(population);
            rouletteWheel.Initialize(population);
        }

        private void ReevaluateFitness(Population<TGene> population)
        {
            population.Chromosomes.OrderByDescending(c => c.Fitness)
                .Zip(Enumerable.Range(0, population.Size), (c, r) => new { Chromosome = c, Rank = r })
                .ForEach(cr => cr.Chromosome.Fitness = population.Size - cr.Rank);
        }

        public override Chromosome<TGene> Select() => rouletteWheel.Select();
    }
}
