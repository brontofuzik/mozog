namespace SimulatedAnnealing.Functions.Perturbation
{
    public abstract class PerturbationFunction<T> : FunctionBase<T>, IPerturbationFunction<T>
    {
        public abstract T[] Perturb(T[] state);
    }
}
