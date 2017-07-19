using Mozog.Utils;
using Mozog.Utils.Math;

namespace GeneticAlgorithm.Functions.Crossover
{
    public class UniformCrossover<TGene> : CrossoverOperator<TGene>
    {
        public override void CrossOver(TGene[] offspring1, TGene[] offspring2)
        {
            for (int i = 0; i < offspring1.Length; i++)
            {
                if (new Probability(0.5))
                {
                    Misc.Swap(ref offspring1[i], ref offspring2[i]);
                }
            }
        }
    }
}