using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Functions.Termination
{
    public class AnyTermination<TGene> : CompositeTermination<TGene>
    {
        public AnyTermination(IEnumerable<ITerminationFunction<TGene>> terminations)
            : base(terminations)
        {
        }

        public override bool ShouldTerminate() => Terminations.Any(t => t.ShouldTerminate());
    }
}
