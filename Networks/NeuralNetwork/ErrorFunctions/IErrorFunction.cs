using NeuralNetwork.MultilayerPerceptron.Backpropagation;

namespace NeuralNetwork.ErrorFunctions
{
    public interface IErrorFunction
    {
        double Evaluate(double[] output, double[] target);
    }

    public interface IDifferentiableErrorFunction : IErrorFunction
    {
        double EvaluateDerivative(BackpropagationNeuron outputNeuron, double target);
    }

    public static class Error
    {
        public static IErrorFunction MSE => new MeanSquaredError();

        public static IErrorFunction CEE => new CrossEntropyError();
    }
}
