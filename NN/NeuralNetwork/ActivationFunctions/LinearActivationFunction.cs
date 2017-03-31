namespace NeuralNetwork.ActivationFunctions
{
    public class LinearActivationFunction : IDerivableActivationFunction
    {
        public double Evaluate(double x) => x;

        public double EvaluateDerivative(double x) => 1.0;

        public override string ToString() => "Lin";
    }
}
