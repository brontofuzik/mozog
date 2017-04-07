namespace NeuralNetwork.ActivationFunctions
{
    // Marker iface
    public interface IActivationFunction
    {
    }

    public interface IActivationFunction1 : IActivationFunction
    {
        double Evaluate(double x);
    }

    public interface IDifferentiableActivationFunction1 : IActivationFunction1
    {
        double EvaluateDerivative(double x);
    }

    public interface IActivationFunction2 : IActivationFunction
    {
        double[] Evaluate(double[] inputs);
    }

    public interface IDifferentiableActivationFunction2 : IActivationFunction2
    {
        double[] EvaluateDerivative(double[] input);
    }
}
