using GeneticAlgorithm.Selectors;

namespace GeneticAlgorithm
{
    public class Parameters<TGene>
    {
        // Functions & operators
        public InitializationFunction<TGene> InitializationFunction { get; set; }
        public ObjectiveFunction<TGene> ObjectiveFunction { get; set; }
        public FitnessFunction FitnessFunction { get; set; }
        public ISelector<TGene> Selector { get; set; }
        public CrossoverOperator<TGene> CrossoverOperator { get; set; }
        public MutationOperator<TGene> MutationOperator { get; set; }
        public TerminationFunction<TGene> TerminationFunction { get; set; }

        public int ChromosomeSize { get; set; }
        public int PopulationSize { get; set; }
        public double CrossoverRate { get; set; }
        public double MutationRate { get; set; }
        public bool Scaling { get; set; }

        public double AcceptableEvaluation { get; set; }
        public int MaxGenerations { get; set; }
    }
}