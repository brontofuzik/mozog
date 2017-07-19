using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Functions.Termination
{
    public class AllTermination<TGene> : CompositeTermination<TGene>
    {
        public AllTermination(IEnumerable<ITerminationFunction<TGene>> terminations)
            : base(terminations)
        {
        }

        public override bool ShouldTerminate() => Terminations.All(t => t.ShouldTerminate());
    }
}
