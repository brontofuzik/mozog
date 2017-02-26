using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Configuration;
using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Termination
{
    public abstract class CompositeTermination<TGene> : ITerminationFunction<TGene>
    {
        protected IList<ITerminationFunction<TGene>> Terminations { get; set; }

        protected CompositeTermination(IEnumerable<ITerminationFunction<TGene>> terminations)
        {
            Terminations = new List<ITerminationFunction<TGene>>(terminations);
        }

        public GeneticAlgorithm<TGene> Algo
        {
            get
            {
                return Terminations.FirstOrDefault()?.Algo;
            }
            set
            {
                Terminations.ForEach(t => t.Algo = value);
            }
        }

        public abstract bool ShouldTerminate();
    }
}
