namespace SimulatedAnnealing.Functions.Perturbation
{
    public interface IPerturbationFunction<T> : IFunction<T>
    {
        T[] Perturb(T[] state);
    }
}
