using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Crossover
{
    public class TwoPointCrossover<TGene> : CrossoverOperator<TGene>
    {
        public override void CrossOver(TGene[] offspring1, TGene[] offspring2)
        {
            // Choose two points from interval [1, chromosomeSize) randomly.
            int point1 = Random.Int(1, offspring1.Length - 1);
            int point2 = Random.Int(point1 + 1, offspring1.Length);

            // Swap all genes from the point1 (including) to the point2 (excluding).
            Misc.SwapArrays(offspring1, point1, offspring2, point1, point2 - point1);
        }
    }
}