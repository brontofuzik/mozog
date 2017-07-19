namespace SimulatedAnnealing.Functions.Objective
{
    public interface IObjectiveFunction<T> : IFunction<T>
    {
        double Evaluate(T[] state);
    }
}