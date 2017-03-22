namespace GeneticAlgorithm.Functions.Fitness
{
    public interface IFitnessFunction<TGene> : IFunction<TGene>
    {
        double EvaluateObjective(Chromosome<TGene> chromosome);

        void EvaluateFitness(Population<TGene> population);

        bool IsAcceptable(double bestEvaluation, double acceptableEvaluation);
    }
}