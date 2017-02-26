namespace GeneticAlgorithm.Functions.Fitness
{
    public interface IFitnessFunction<TGene> : IFunction<TGene>
    {
        double EvaluateObjective(Chromosome<TGene> chromosome);

        void EvaluateFitness(Population<TGene> population);

        void UpdateBestChromosome(Chromosome<TGene> chromosome, ref Chromosome<TGene> bestChromosome);

        bool IsAcceptable(double bestEvaluation, double acceptableEvaluation);
    }
}