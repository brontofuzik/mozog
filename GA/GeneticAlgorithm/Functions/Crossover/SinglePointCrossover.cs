using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Crossover
{
    // The binary-coded one-point (1-PX) crossover function.
    // ---
    // Crossover Operators (S. Malkos)
    // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
    public class SinglePointCrossover<TGene> : CrossoverOperator<TGene>
    {
        public override void CrossOver(TGene[] offspring1, TGene[] offspring2)
        {
            // Choose a point from itnerval [1, chromosomeSize) randomly.
            int point = StaticRandom.Int(1, offspring1.Length);

            // Swap all genes from the point (including) to the end.
            Misc.SwapArrays(offspring1, point, offspring2, point, offspring1.Length - point);
        }
    }
}