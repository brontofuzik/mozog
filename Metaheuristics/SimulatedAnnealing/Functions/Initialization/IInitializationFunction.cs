namespace SimulatedAnnealing.Functions.Initialization
{
    public interface IInitializationFunction<T> : IFunction<T>
    {
        T[] Initialize(int dimension);
    }
}
