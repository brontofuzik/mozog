namespace SimulatedAnnealing.Functions.Initialization
{
    public abstract class InitializationFunction<T> : FunctionBase<T>, IInitializationFunction<T>
    {
        public abstract T[] Initialize(int dimension);
    }
}