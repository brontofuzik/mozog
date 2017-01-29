namespace GeneticAlgorithm 
{
    public delegate TGene[] InitializationFunction<TGene>(GeneticAlgorithm<TGene> args);

    public delegate double FitnessFunction(double evaluation, double averageEvaluation, Objective objective);

    public delegate void CrossoverOperator<TGene>(TGene[] offspring1, TGene[] offspring2, GeneticAlgorithm<TGene> args);

    public delegate void MutationOperator<TGene>(TGene[] genes, GeneticAlgorithm<TGene> args);

    public delegate bool TerminationFunction<TGene>(int generation, double bestEvaluation, GeneticAlgorithm<TGene> args);
}