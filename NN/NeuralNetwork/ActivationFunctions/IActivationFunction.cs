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

    public static class Activation
    {
        public static IActivationFunction Linear => new LinearFunction();

        public static IActivationFunction Sigmoid => new SigmoidFunction();

        public static IActivationFunction Softmax=> new SoftmaxFunction();

        public static IActivationFunction Tanh => new HyperbolicTangent();
    }
}
