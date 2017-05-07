namespace SimulatedAnnealing.Functions
{
    public interface IFunction<T>
    {
        SimulatedAnnealing<T> Algo { get; set; }
    }

    public abstract class FunctionBase<T> : IFunction<T>
    {
        public SimulatedAnnealing<T> Algo { get; set; }
    }
}